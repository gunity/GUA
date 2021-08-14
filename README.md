# GUA

* [About](#about)
* [Installation](#installation)
* [GSystem](#gsystem)
* [GState](#gstate)
  * [Simple state](#simple-state)
  * [Behavioral states](#behavioral-states)
* [GDataPool](#gdatapool)
* [GEventPool](#geventpool)
* [GInvoke](#ginvoke)
* [Extensions](#extensions)
  * [Singleton](#singleton)
  * [SetListener](#setlistener)

## About
A simple package, following the rules of which you can write
a more supportable and extensible application. GUA uses the principles
of single entry into the application, independence of its systems, variability.

## Installation
This repository can be installed as unity module directly from Git URL
```text
https://github.com/gunity/GUA.git
```

![Installation](https://github.com/gunity/GUA/blob/main/Content/package_manager.gif)

## GSystem
A single-entry principle is used.
The whole code is divided into parts - systems.
The system must be inherited from the `MonoSystem`.
```c#
public class SomeSystem : MonoSystem
{
    public SomeSystem(bool enabled) : base(enabled) { }
}
```
In the starter file you must add the system.
```c#
_system.Add(new SomeSystem(true));
```
where `true` - the system is initially enabled.
```c#
public class SomeSystem : MonoSystem
{
    public SomeSystem(bool enabled) : base(enabled)
    {
        // will be executed once when the system is initialized
    }
    
    protected override void Start()
    {
        // will be executed once on the next frame after the first enabling of the system
    }

    public override void Run()
    {
        // analogue Update
    }

    public override void FixedRun()
    {
        // analogue FixedUpdate
    }
}
```
If your system is disabled, you can turn it on using the `Enabled` field
```c#
public class SomeSystem : MonoSystem
{
    public SomeSystem(bool enabled) : base(enabled)
    {
        GEventPool.AddListener<SimpleEvent>(simpleEvent => Enabled = true);
    }
    
    public override void Run()
    {
        Debug.Log("The system is now enabled");
    }
}
```

## GState

### Simple state

You can save the enum state with GState
```c#
public enum SomeState
{
    First,
    Second,
    Third
}
```
```c#
GState.SetState(SomeState.Second);
```

To get the enum state
```c#
var state = GState.GetState<SomeState>();
```

### Behavioral states

Also when creating a state, you can bind behavior to it. 
Create behaviors that inherit from the same class
```c#
public class MainBehaviour
{
    public virtual void SomeMethod()
    {
        Debug.Log("State is not specifically defined");
    }
}
```
```c#
public class FirstBehaviour : MainBehaviour
{
    public override void SomeMethod()
    {
        Debug.Log("First Behaviour");
    }
}
```
```c#
public class SecondBehaviour : MainBehaviour
{
    public override void SomeMethod()
    {
        Debug.Log("Second Behaviour");
    }
}
```
Declare the state and specify the default behavior
```c#
public PlayerSystem(bool enabled) : base(enabled)
{
    // SomeState.Second - initial state
    // new MainBehaviour() - default behavior
    GState.SetState(SomeState.Second, new MainBehaviour());
                
    // Bind state to behavior
    GState.RegisterState(SomeState.First, new FirstBehaviour());
    GState.RegisterState(SomeState.Second, new SecondBehaviour());
}

public override void Run()
{
    GState.GetExecutable<MainBehaviour>().SomeMethod();
}
```
If you have not bound a state, the default behavior will be executed.

## GDataPool

You can pass information to each system through the constructor,
but then things will quickly become cluttered. 
That's why the data container is created. 
A container that stores data by unique types.

Create a unique type field in the starter-file
```c#
[SerializeField] private SomeData someData;
```
Injection
```c#
GDataPool.Set(someData);
```
Getting data
```c#
private readonly SomeData someData = GDataPool.Get<SomeData>();
```

## GEventPool

Systems can communicate with each other with event structures.

Create event-structure
```c#
public struct SomeEvent
{
    public int SomeInteger;
    public string SomeString;
}
```
Add message listener
```c#
GEventPool.AddListener<SomeEvent>(some =>
{
    Debug.Log($"Integer: {some.SomeInteger}; String: {some.SomeString}");
});
```
Send message
```c#
GEventPool.SendMessage(new SomeEvent
{
    SomeInteger = 123,
    SomeString = "string"
});
```
See console
```text
Integer: 123 String: string
```

## GInvoke

A way to delay code execution.
The example below displays "Hello, World!" in the console after 5 seconds,
but if you press the space bar, the execution is interrupted.

```c#
private readonly GInvoke _gInvoke;

public SomeSystem(bool enabled) : base(enabled)
{
    _gInvoke = new GInvoke();
    _gInvoke.Delay(() =>
    {
        Debug.Log("Hello, World!");
    }, 5000);
}

public override void Run()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        _gInvoke.Stop();
    }
}
```

## Extensions

### Singleton

```c#
public class SomeClass : Singleton<SomeClass>
{
    // now it's Singleton class
}
```

### SetListener
Instead
```c#
unityEvent.RemoveAllListeners();
unityEvent.AddListener(SomeMethod);            
```
You could write it like this          
```c#
unityEvent.SetListener(SomeMethod);
```
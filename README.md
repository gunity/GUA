# GUA
> Simple architecture that is designed to use some of the programming patterns

* [Installation](#installation)
* [GSystem](#gsystem)
* [GDataPool](#gdatapool)
* [GEventPool](#geventpool)
* [GInvoke](#ginvoke)
* [Singleton](#singleton)

## Installation
This repository can be installed as unity module directly from Git URL
```
https://github.com/gunity/GUA.git
```

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
where `true` - is whether the system is initially enabled.
```c#
public class SomeSystem : MonoSystem
{
    public SomeSystem(bool enabled) : base(enabled)
    {
        // is performed when system initialization
    }
    
    protected override void Start()
    {
        // is performed once when the system is switched on
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
## GDataPool

Create a unique type field in the starter-file
```c#
[SerializeField] private SomeData someData;
```
Injection
```c#
GDataPool.Set(someData);
```
Getting dependency
```c#
private readonly SomeData someData = GDataPool.Get<SomeData>();
```

## GEventPool

Create structure-event
```c#
public struct SomeEvent
{
    public int SomeInteger;
    public string SomeString;
}
```
Send message
```c#
GEventPool.SendMessage(new SomeEvent
{
    SomeInteger = 123,
    SomeString = "string"
});
```
Add message listener
```c#
GEventPool.AddListener<SomeEvent>(some =>
{
    Debug.Log($"Integer: {some.SomeInteger}; String: {some.SomeString}");
});
```

## GInvoke
> An easier way to use coroutine
> 
This will print the message to the console after 5 seconds
```c#
GInvoke.Instance.Delay(() =>
{
    Debug.Log("Hello, World!");
}, 5f);
```
# GUA

* [About](#about)
* [Installation](#installation)
* [GSystem](#gsystem)
* [GDataPool](#gdatapool)
* [GEventPool](#geventpool)
* [GInvoke](#ginvoke)
* [Extensions](#extensions)

> the manual is incomplete at this moment, sorry. I will correct it soon :)

## About
A simple package, following the rules of which you can write
a more supportable and extensible application. GUA uses the principles
of single entry into the application, independence of its systems, variability.

## Installation
This repository can be installed as unity module directly from Git URL

>https://github.com/gunity/GUA.git

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
## GDataPool

Date Injection. A container that stores data by unique types.

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
> Integer: 123 String: string

## GInvoke

coming soon

## Extensions

coming soon
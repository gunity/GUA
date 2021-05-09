# GUA
> Simple architecture that is designed to use some of the programming patterns

* [Installation](#installation)
* [GSystem](#gsystem)
* [GDataPool](#gdatapool)
* [GEventPool](#geventpool)
* [Singleton](#singleton)

## Installation
This repository can be installed as unity module directly from Git URL
```
https://github.com/gunity/GUA.git
```

## GSystem
User class should implements `IStartSystem`, `IRunSystem`, `IFixedRunSystem` interfaces:
```csharp
public class SomeSystem : IStartSystem, IRunSystem, IFixedRunSystem
{
    public void Start() { }

    public void Run() { }

    public void FixedRun() { }
}
```

## GDataPool
> Dependency injection

Create a unique type field in the starter-file
```csharp
[SerializeField] private SomeData someData;
```
Injection
```csharp
GDataPool.Set(someData);
```
Getting dependency
```csharp
private readonly SomeData someData = GDataPool.Get<SomeData>();
```

## GEventPool
> Observer

Create structure-event
```csharp
public struct SomeEvent
{
    public int SomeInteger;
    public string SomeString;
}
```
Send message
```csharp
GEventPool.SendMessage(new SomeEvent
{
    SomeInteger = 123,
    SomeString = "string"
});
```
Add message listener
```csharp
GEventPool.AddListener<SomeEvent>(some =>
{
    Debug.Log($"Integer: {some.SomeInteger}; String: {some.SomeString}");
});
```

## Singleton
To make a singleton, you need this
```csharp
public class SomeMonobeh : MonoBehaviour
```
Replace with this
```csharp
public class SomeMonobeh : Singleton<SomeMonobeh>
```

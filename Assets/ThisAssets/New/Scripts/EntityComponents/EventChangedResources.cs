using Unity.Entities;
using System;
public class EventChangedResources : IComponentData
{
    public event Action<Resource, int> TargetEvent;
}
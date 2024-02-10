using Unity.Entities;
using System;
public class EventChangedResources : IComponentData
{
    public Action TargetEvent;
}
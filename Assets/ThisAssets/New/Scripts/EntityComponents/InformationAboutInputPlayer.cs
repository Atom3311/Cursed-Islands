using Unity.Entities;
using Unity.Mathematics;
using System;
public struct InformationAboutInputPlayer : IComponentData
{
    public bool ClickDown;
    public bool ClickUp;
    public bool Hold;
    public float2 MousePosition;
}
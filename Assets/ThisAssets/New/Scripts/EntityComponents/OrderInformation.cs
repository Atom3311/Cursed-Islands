using Unity.Entities;
using Unity.Mathematics;
public struct OrderInformation : IComponentData
{
    public Order DuringOrder;
    public Entity TargetEntity;
    public float3 TargetPoint;
}
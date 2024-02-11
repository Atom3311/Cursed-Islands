using Unity.Entities;

struct Collector : IComponentData
{
    public Resource TargetResource;
    public float Speed;
    public float Range;
}
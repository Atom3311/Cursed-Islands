using Unity.Entities;

struct Collector : IComponentData
{
    public Resource TargetResourceType;
    public float Speed;
    public float Range;
    public ResourceInformation TargetResourceEntity;
}
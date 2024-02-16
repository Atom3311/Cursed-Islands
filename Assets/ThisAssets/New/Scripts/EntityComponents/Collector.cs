using Unity.Entities;

struct Collector : IComponentData
{
    public Resource TargetResourceType;
    public float NeededTime;
    public float DuringTime;
    public float Range;
    public Entity TargetResourceEntity;
}
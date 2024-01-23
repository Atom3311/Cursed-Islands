using Unity.Entities;
public readonly partial struct PackageOfMovableUnit : IAspect
{
    public readonly RefRO<MovableUnit> Unit;
    public readonly RefRO<SpeedComponent> SpeedComponent;
    public readonly RefRW<MovePoint> MovePoint;
}
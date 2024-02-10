using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
partial struct MovingAllMovableUnits : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach ((
            PackageOfMovableUnit package,
            RefRW<LocalTransform> transform,
            RefRW<AttentionPoint> attentionPoint) in
            SystemAPI.Query<
                PackageOfMovableUnit,
                RefRW<LocalTransform>,
                RefRW<AttentionPoint>>())
        {
            float3? targetPoint = package.MovePoint.ValueRO.PointInWorld;
            attentionPoint.ValueRW.Point = targetPoint;

            if (!targetPoint.HasValue)
                continue;

            float3 position = transform.ValueRO.Position;

            targetPoint = new float3
                (
                    targetPoint.Value.x,
                    position.y,
                    targetPoint.Value.z
                );


            float3 direction = targetPoint.Value - position;

            direction = math.normalize(direction);

            float move = package.SpeedComponent.ValueRO.Speed * SystemAPI.Time.DeltaTime;

            if (math.distance(position, targetPoint.Value) <= move)
            {
                transform.ValueRW.Position = targetPoint.Value;
                package.MovePoint.ValueRW.PointInWorld = null;
                attentionPoint.ValueRW.Point = null;
            }
            else
            {
                transform.ValueRW.Position += direction * move;
            }
        }
    }
}
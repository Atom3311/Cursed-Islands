using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
partial struct MovingAllMovableUnits : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach ((
            PackageOfMovableUnit package,
            RefRW<LocalTransform> transform) in
            SystemAPI.Query<
                PackageOfMovableUnit,
                RefRW<LocalTransform>>())
        {
            float3? targetPoint = package.MovePoint.ValueRO.PointInWorld;
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
            transform.ValueRW = transform.ValueRO.WithRotation(quaternion.LookRotation(targetPoint.Value - position, math.up()));
            if (math.distance(position, targetPoint.Value) <= move)
            {
                transform.ValueRW.Position = targetPoint.Value;
                package.MovePoint.ValueRW.PointInWorld = null;
            }
            else
            {
                transform.ValueRW.Position += direction * move;
            }
            
        }
    }
}
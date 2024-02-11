using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public partial struct ObservationOfPoint : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach ((
            RefRO<AttentionPoint> targetPoint,
            RefRW<LocalTransform> transform) in SystemAPI.Query<
                RefRO<AttentionPoint>,
                RefRW<LocalTransform>>())
        {
            if (!targetPoint.ValueRO.Point.HasValue)
                continue;

            float3 newPoint = targetPoint.ValueRO.Point.Value;
            newPoint.y = transform.ValueRO.Position.y;
            transform.ValueRW = transform.ValueRO.WithRotation(quaternion.LookRotation(newPoint - transform.ValueRO.Position, math.up()));

        }
    }
}
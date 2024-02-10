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

            transform.ValueRW = transform.ValueRO.WithRotation(quaternion.LookRotation(targetPoint.ValueRO.Point.Value - transform.ValueRO.Position, math.up()));

        }
    }
}
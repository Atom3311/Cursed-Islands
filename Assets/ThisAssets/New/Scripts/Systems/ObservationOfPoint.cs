using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public partial struct ObservationOfPoint : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach ((
            RefRW<AttentionPoint> targetPoint,
            RefRW<LocalTransform> transform,
            RefRO<HealthState> healthState) in SystemAPI.Query<
                RefRW<AttentionPoint>,
                RefRW<LocalTransform>,
                RefRO<HealthState>>())
        {
            if (healthState.ValueRO.IsDead)
                continue;

            if (!targetPoint.ValueRO.Point.HasValue)
                continue;

            float3 newPoint = targetPoint.ValueRO.Point.Value;
            newPoint.y = transform.ValueRO.Position.y;

            if (Equals(newPoint, transform.ValueRO.Position))
            {
                targetPoint.ValueRW.Point = null;
                return;
            }

            transform.ValueRW = transform.ValueRO.WithRotation(quaternion.LookRotation(newPoint - transform.ValueRO.Position, math.up()));

        }
    }
}
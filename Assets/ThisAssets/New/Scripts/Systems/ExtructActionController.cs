using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
public partial struct ExtructActionController : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (actionTag, healthState, attantionPoint, collector, transform, package, entity) in SystemAPI.Query<
            RefRO<ExtructAction>,
            RefRO<HealthState>,
            RefRW<AttentionPoint>,
            RefRO<Collector>,
            RefRW<LocalTransform>,
            PackageOfMovableUnit>()
            .WithEntityAccess())
        {
            if (healthState.ValueRO.IsDead)
                continue;

            Entity target = collector.ValueRO.TargetResourceEntity;

            if (target == Entity.Null)
                continue;

            float3 targetPosition = SystemAPI.GetComponent<LocalTransform>(target).Position;
            attantionPoint.ValueRW.Point = targetPosition;

            float3 position = transform.ValueRO.Position;
            float range = collector.ValueRO.Range;

            if (math.distance(position, targetPosition) > range)
                continue;

            package.MovePoint.ValueRW.PointInWorld = null;

            ecb.AddComponent<Extructing>(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}
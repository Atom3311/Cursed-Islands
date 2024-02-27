using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
[UpdateAfter(typeof(BuilderHealthStateControll))]
public partial struct BuilderConductor : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var(builder, packageOfMovableUnit, attentionPoint, transform, entity) in SystemAPI.Query<
            RefRW<Builder>,
            PackageOfMovableUnit,
            RefRW<AttentionPoint>,
            RefRO<LocalTransform>>()
            .WithEntityAccess())
        {
            Entity foundationEntity = builder.ValueRO.TargetFoundation;

            if (foundationEntity == Entity.Null)
                continue;

            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(foundationEntity);

            attentionPoint.ValueRW.Point = targetTransform.Position;

            float distance = math.distance(transform.ValueRO.Position, targetTransform.Position);

            if (distance > Constants.RangeForBuild)
            {
                packageOfMovableUnit.MovePoint.ValueRW.PointInWorld = targetTransform.Position;
                ecb.RemoveComponent<Build>(entity);
            }
            else
            {
                packageOfMovableUnit.MovePoint.ValueRW.PointInWorld = null;
                ecb.AddComponent<Build>(entity);
            }
        }
        ecb.Playback(state.EntityManager);
    }
}
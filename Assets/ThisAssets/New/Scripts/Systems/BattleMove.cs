using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
[UpdateAfter(typeof(BattleController))]
public partial struct BattleMove : ISystem
{
    private void OnUpdate(ref SystemState state)
    {

        foreach (var(healthState, battleComponent, transform, moveComponents, attentionPoint, entity) in SystemAPI.Query<
            RefRO<HealthState>,
            RefRW<BattleComponent>,
            RefRO<LocalTransform>,
            PackageOfMovableUnit,
            RefRW<AttentionPoint>>()
            .WithEntityAccess())
        {
            if (healthState.ValueRO.IsDead)
            {
                moveComponents.MovePoint.ValueRW.PointInWorld = null;
                continue;
            }
            
            Entity targetEntity = battleComponent.ValueRO.Target;

            if (targetEntity == Entity.Null)
                continue;

            HealthState targetHealthState = SystemAPI.GetComponent<HealthState>(targetEntity);
            if (targetHealthState.IsDead)
            {
                moveComponents.MovePoint.ValueRW.PointInWorld = null;
                continue;
            }
            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

            float3 position = transform.ValueRO.Position;
            float3 targetPosition = targetTransform.Position;
            attentionPoint.ValueRW.Point = targetPosition;

            if (SystemAPI.HasComponent<AttackingUnit>(entity) || DistanceIsNormal())
            {
                moveComponents.MovePoint.ValueRW.PointInWorld = null;
                continue;
            }
            else
            {
                moveComponents.MovePoint.ValueRW.PointInWorld = targetPosition;
            }


            bool DistanceIsNormal()
            {
                return math.distance(position, targetPosition) <= battleComponent.ValueRO.RangeOfAttack;
            }
        }

    }
}
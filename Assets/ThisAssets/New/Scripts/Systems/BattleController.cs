using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
[UpdateAfter(typeof(OrderToAttackc))]
[UpdateAfter(typeof(SearchPlayerUnits))]
public partial struct BattleController : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (battleComponent, transform, healthState, entity) in SystemAPI.Query<
            RefRW<BattleComponent>,
            RefRO<LocalTransform>,
            RefRO<HealthState>>()
            .WithEntityAccess())
        {
            if (healthState.ValueRO.IsDead)
                continue;

            Entity targetEntity = battleComponent.ValueRO.Target;
            if (targetEntity == Entity.Null)
            {
                ecb.RemoveComponent<AttackingUnit>(entity);
                battleComponent.ValueRW.DuringTime = 0;
                continue;
            }

            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

            float3 position = transform.ValueRO.Position;
            float3 targetPosition = targetTransform.Position;

            bool isAttacking = SystemAPI.HasComponent<AttackingUnit>(entity);

            if (DistanceIsNormal())
            {
                ecb.AddComponent<AttackingUnit>(entity);
                isAttacking = true;
            }

            if (!isAttacking)
            {
                battleComponent.ValueRW.DuringTime = 0;
                continue;
            }
                

            HealthState targetHealthState = SystemAPI.GetComponent<HealthState>(targetEntity);

            if (battleComponent.ValueRO.DuringTime >= battleComponent.ValueRO.TimeForAttack)
            {
                battleComponent.ValueRW.DuringTime = 0;

                targetHealthState.Health -= battleComponent.ValueRO.Power;
                SystemAPI.SetComponent(targetEntity, targetHealthState);

                if (!DistanceIsNormal() && targetHealthState.IsDead)
                {
                    ecb.RemoveComponent<AttackingUnit>(entity);
                }

            }
            else
            {
                battleComponent.ValueRW.DuringTime += SystemAPI.Time.DeltaTime;
            }


            bool DistanceIsNormal()
            {
                return math.distance(position, targetPosition) <= battleComponent.ValueRO.RangeOfAttack;
            }

        }
        ecb.Playback(state.EntityManager);

    }
}
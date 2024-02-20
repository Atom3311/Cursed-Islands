using Unity.Entities;
using Unity.Physics;
using Unity.Collections;
using Unity.Transforms;
[UpdateAfter(typeof(EnemiesHealthControll))]
partial struct SearchPlayerUnits : ISystem
{

    private void OnUpdate(ref SystemState state)
    {
        CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

        foreach (var (ownerComponent, healthState, battleComponent, transform, entity) in SystemAPI.Query<
                    RefRO<OwnerComponent>,
                    RefRO<HealthState>,
                    RefRW<BattleComponent>,
                    RefRO<LocalTransform>>()
                    .WithEntityAccess())
        {
            if (ownerComponent.ValueRO.Owner == OwnersInGame.Player)
                continue;

            if (healthState.ValueRO.IsDead)
                continue;

            Entity targetEntity = battleComponent.ValueRO.Target;
            if (targetEntity != Entity.Null)
                continue;

            NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

            collisionWorld.OverlapSphere(transform.ValueRO.Position, battleComponent.ValueRO.RangeOfViewing, ref hits, CollisionFilter.Default);

            foreach (DistanceHit hit in hits)
            {
                Entity hitTargetEntity = hit.Entity;
                if(SystemAPI.HasComponent<OwnerComponent>(hitTargetEntity) &&
                    SystemAPI.HasComponent<HealthState>(hitTargetEntity))
                {
                    OwnerComponent targetOwnerComponent = SystemAPI.GetComponent<OwnerComponent>(hitTargetEntity);
                    
                    if (targetOwnerComponent.Owner != OwnersInGame.Player)
                        continue;

                    HealthState targetHealthState = SystemAPI.GetComponent<HealthState>(hitTargetEntity);

                    if (targetHealthState.IsDead)
                        continue;

                    battleComponent.ValueRW.Target = hitTargetEntity;
                    break;
                }
            }
        }
    }
}
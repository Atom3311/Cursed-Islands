using Unity.Entities;

public partial struct EnemiesHealthControll : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach (var battleComponent in SystemAPI.Query<
            RefRW<BattleComponent>>())
        {
            Entity targetEntity = battleComponent.ValueRO.Target;
            if (targetEntity == Entity.Null)
                continue;

            HealthState targetHealth = SystemAPI.GetComponent<HealthState>(targetEntity);

            if (targetHealth.IsDead)
                battleComponent.ValueRW.Target = Entity.Null;
        }
    }
}
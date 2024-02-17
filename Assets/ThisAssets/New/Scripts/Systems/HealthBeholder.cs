using Unity.Entities;

public partial struct HealthBeholder : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach (var alive in SystemAPI.Query<RefRW<HealthState>>())
        {
            if (alive.ValueRO.Health <= 0)
                alive.ValueRW.IsDead = true;
        }
    }
}
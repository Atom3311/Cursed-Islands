using Unity.Entities;
public struct HealthState : IComponentData
{
    public int Health;
    public bool IsDead;
    public float DuringDeadTime;
}
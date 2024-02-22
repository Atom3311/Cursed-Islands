using Unity.Entities;
public struct HealthState : IComponentData
{
    public int Health;
    public bool IsDead
    {
        get
        {
            return Health <= 0;
        }
    }
    public float DuringDeadTime;
}
using Unity.Entities;

public struct BattleComponent : IComponentData
{
    public int Power;
    public float RangeOfAttack;
    public float RangeOfViewing;
    public float TimeForAttack;
    public float DuringTime;
    public Entity Target;
}
using UnityEngine;
using Unity.Entities;
[RequireComponent(typeof(HullOfMovableUnit))]
public class BattleUnit : MonoBehaviour
{
    [Min(0)] public int Power;
    [Min(0)] public float RangeForAttack;
    [Min(0)] public float RangeForViewing;
    [Min(0)] public float TimeForAttack;

    private class ThisBaker : Baker<BattleUnit>
    {
        public override void Bake(BattleUnit authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(targetEntity, new BattleComponent()
            {
                Power = authoring.Power,
                RangeOfAttack = authoring.RangeForAttack,
                RangeOfViewing = authoring.RangeForViewing,
                TimeForAttack = authoring.TimeForAttack
            });
        }
    }
}
using UnityEngine;
using Unity.Entities;
public class UnitWithHealth : MonoBehaviour
{
    [Min(0)] public int Health;
    private class ThisBaker : Baker<UnitWithHealth>
    {
        public override void Bake(UnitWithHealth authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(targetEntity, new HealthState() { Health = authoring.Health });
        }
    }
}

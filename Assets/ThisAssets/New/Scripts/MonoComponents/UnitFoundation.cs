using UnityEngine;
using Unity.Entities;
public class UnitFoundation : MonoBehaviour
{
    private class ThisBaker : Baker<UnitFoundation>
    {
        public override void Bake(UnitFoundation authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent<Unit>(targetEntity);
            AddComponent<Foundation>(targetEntity);
        }
    }
}

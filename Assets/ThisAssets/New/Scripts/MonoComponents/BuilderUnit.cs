using UnityEngine;
using Unity.Entities;
[RequireComponent(typeof(HullOfMovableUnit))]
public class BuilderUnit : MonoBehaviour
{
    private class ThisBaker : Baker<BuilderUnit>
    {
        public override void Bake(BuilderUnit authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Builder>(targetEntity);
        }
    }
}
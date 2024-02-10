using UnityEngine;
using Unity.Entities;
public class HullOfMovableUnit : MonoBehaviour
{
    public OwnersInGame Owner;
    [Min(0)] public float Speed;
    private class ThisBaker : Baker<HullOfMovableUnit>
    {
        public override void Bake(HullOfMovableUnit authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Unit>(targetEntity);
            AddComponent<MovableUnit>(targetEntity);
            AddComponent(targetEntity, new OwnerComponent() { Owner = authoring.Owner });
            AddComponent(targetEntity , new SpeedComponent() { Speed = authoring.Speed });
            AddComponent<MovePoint>(targetEntity);
            AddComponent<AttentionPoint>(targetEntity);
        }
    }
}
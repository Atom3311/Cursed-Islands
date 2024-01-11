using Unity.Entities;
using UnityEngine;
public class Example_B_ComponentOfMove : Baker<Example_Move>
{
    public override void Bake(Example_Move authoring)
    {
        Entity thisEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(thisEntity, new C_SpeedOfMovement { Speed = authoring.Speed });
        AddComponent(thisEntity , new C_OrientationInGame { Orientation = authoring._direction});
    }
}

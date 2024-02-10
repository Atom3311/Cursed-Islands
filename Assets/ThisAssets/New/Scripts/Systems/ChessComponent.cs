using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

public partial class TreasureChestSystem : ISystem
{
    protected void OnUpdate(ref SystemState state)
    {
        foreach ((Entity entity, ref PhysicsCollider collider) =>
            {
                collider.GetContacts(Allocator.Temp, (ref PhysicsContact contact) =>
                {
                    Entity otherEntity = contact.Entity;

                    if (HasComponent<OwnerComponent>(otherEntity))
                    {
                        OwnerComponent ownerComponent = GetComponent<OwnerComponent>(otherEntity);
                        if (owner.ValueRO.Owner == OwnersInGame.Playe)
                        {
                            UnityEngine.Debug.Log("Игрок коснулся сундука!");
                        }
                    }
                });

            }) ;
    }
}

using Unity.Entities;
public class B_MoveComponentForPlayerUnit : Baker<M_MoveComponentForPlayerUnit>
{
    public override void Bake(M_MoveComponentForPlayerUnit authoring)
    {
        Entity thisEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<C_SpeedOfMovement>(thisEntity, new C_SpeedOfMovement(authoring.Speed));
        AddComponent<C_MoveOnPoint>(thisEntity);
    }
}

using Unity.Entities;
class B_UnitWithOwner : Baker<M_UnitWithOwner>
{
    public override void Bake(M_UnitWithOwner authoring)
    {
        Entity targetEntity = GetEntity(TransformUsageFlags.None);
        AddComponent(targetEntity, new C_Owner(authoring.Owner));
    }
}
using Unity.Entities;
public struct C_Owner : IComponentData
{
    public OwnersInGame Owner { get; private set; }
    public C_Owner(OwnersInGame owner)
    {
        Owner = owner;
    }
}
using Unity.Entities;
public struct ResourceInformation : IComponentData
{
    public Resource Type;
    public byte Count;
}
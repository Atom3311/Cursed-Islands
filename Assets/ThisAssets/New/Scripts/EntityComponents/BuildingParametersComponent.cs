using Unity.Entities;

public struct BuildingParametersComponent : IComponentData
{
    public Entity Building;
    public int Gold;
    public int Wood;
    public int Food;
}
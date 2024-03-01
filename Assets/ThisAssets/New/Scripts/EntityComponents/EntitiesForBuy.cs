using Unity.Entities;
public class EntitiesForBuy : IComponentData
{
    public Entity[] Entities;
    public UnitCost[] UnitsCost;
}
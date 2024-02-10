using Unity.Entities;
using Unity.Mathematics;
public struct InformationAboutResources : IComponentData
{
    public int Gold { get; private set; }
    public int Wood { get; private set; }
    public int Food { get; private set; }

    public void AddValue(Resource type, int value)
    {
        switch(type)
        {
            case Resource.Gold: Gold = math.max(0, Gold + value); break;
            case Resource.Wood: Wood = math.max(0, Wood + value); break;
            case Resource.Food: Food = math.max(0, Food + value); break;
        }
    }
    public void SetValue(Resource type, int value)
    {
        switch (type)
        {
            case Resource.Gold: Gold = math.max(0, value); break;
            case Resource.Wood: Wood = math.max(0, value); break;
            case Resource.Food: Food = math.max(0, value); break;
        }
    }
}
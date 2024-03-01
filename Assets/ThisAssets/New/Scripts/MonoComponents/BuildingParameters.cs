using UnityEngine;
using Unity.Entities;
using System;
using System.Collections.Generic;
public class BuildingParameters : MonoBehaviour
{
    public GameObject Building;
    public int Gold;
    public int Wood;
    public int Food;
    [SerializeField] private UnitForBuy[] _units = new UnitForBuy[0];
    private class ThisBaker : Baker<BuildingParameters>
    {
        public override void Bake(BuildingParameters authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);

            Entity buildingEntity = GetEntity(authoring.Building, TransformUsageFlags.None);
            AddComponent(targetEntity, new BuildingParametersComponent()
            {
                Building = buildingEntity,
                Gold = authoring.Gold,
                Wood = authoring.Wood,
                Food = authoring.Food
            });

            int countOfUnits = authoring._units.Length;
            Entity[] entitiesOfUnitsForBuy = new Entity[countOfUnits];
            UnitCost[] unitsCost = new UnitCost[countOfUnits];

            foreach(UnitForBuy unitForBuy in authoring._units)
            {
                Entity entity = GetEntity(unitForBuy.TargetUnit, TransformUsageFlags.Dynamic);
                entitiesOfUnitsForBuy[(int)unitForBuy.Type] = entity;
                unitsCost[(int)unitForBuy.Type] = unitForBuy.Cost;
            }

            AddComponentObject(targetEntity, new EntitiesForBuy()
            {
                Entities = entitiesOfUnitsForBuy,
                UnitsCost = unitsCost
            });
        }
    }
    [Serializable]
    private struct UnitForBuy
    {
        public GameObject TargetUnit;
        public BuySelection Type;
        public UnitCost Cost;
    }
}
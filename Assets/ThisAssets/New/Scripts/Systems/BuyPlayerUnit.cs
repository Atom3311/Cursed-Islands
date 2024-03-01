using UnityEditor;
using Unity.Entities;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Transforms;
using System.Collections.Generic;
using Unity.Mathematics;
[UpdateAfter(typeof(OrdersController))]
[UpdateAfter(typeof(InputHandler))]
public partial class BuyPlayerUnit : SystemBase
{
    private bool _isInit;
    private BuyUnitsParametersForUI _buyUIParameters;
    private Entity _targetEntity;
    private LocalTransform _targetTransform;
    private List<Button> _buttonsForSelection;
    private EntitiesForBuy _entititesForBuy;
    private InformationAboutResources _duringResources;
    protected override void OnCreate()
    {
        RequireForUpdate<OrderInformation>();
        RequireForUpdate<InformationAboutInputPlayer>();
        RequireForUpdate<EntitiesForBuy>();

        if (!EditorSettings.enterPlayModeOptionsEnabled)
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.isSubScene)
                    return;

                Start();
            };
        }
        else
        {
            Start();
        }

        void Start()
        {
            _buyUIParameters = Object.FindObjectOfType<BuyUnitsParametersForUI>(true);
            _buyUIParameters.Init();
            _buttonsForSelection = new List<Button>(_buyUIParameters.ButtonsForSelection);
            _buyUIParameters.ButtonForClose.onClick.AddListener(CloseSelection);
            _isInit = true;

        }
    }
    protected override void OnUpdate()
    {
        if (!_isInit)
            return;

        OrderInformation orderInformation = SystemAPI.GetSingleton<OrderInformation>();
        var input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        _entititesForBuy = SystemAPI.ManagedAPI.GetSingleton<EntitiesForBuy>();
        _duringResources = SystemAPI.GetSingleton<InformationAboutResources>();

        if (orderInformation.DuringOrder == Order.None || orderInformation.DuringOrder != Order.Buy)
            return;

        if (PointOnScreen.PointOnUIElement(input.MousePosition))
            return;

        _targetEntity = orderInformation.TargetEntity;
        _targetTransform = SystemAPI.GetComponent<LocalTransform>(_targetEntity);
        CloseSelection();
        OpenSelection();
    }
    private void OpenSelection()
    {
        _buyUIParameters.TargetUI.SetActive(true);
        foreach (Button button in _buttonsForSelection)
        {
            button.onClick.AddListener(() =>
            {
                int typeIndex = _buttonsForSelection.IndexOf(button);
                Entity targetEntity = _entititesForBuy.Entities[typeIndex];
                UnitCost cost = _entititesForBuy.UnitsCost[typeIndex];
                float3 position = _targetTransform.Position + math.back() * 3;
                AddEvent(targetEntity, cost, position);
            });
        }
    }
    private void CloseSelection()
    {
        _buyUIParameters.TargetUI.SetActive(false);
        Button[] buttonsForSelection = _buyUIParameters.ButtonsForSelection;
        foreach (Button button in buttonsForSelection)
        {
            button.onClick.RemoveAllListeners();
        }
    }
    private void AddEvent(Entity entityForInit, UnitCost cost, float3 position)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        RefRW<PlayerBuilding> building = SystemAPI.GetComponentRW<PlayerBuilding>(_targetEntity);

        if(building.ValueRO.Residents < Constants.MaxResidents && TryBuy())
        {
            Entity createdEntity = ecb.Instantiate(entityForInit);
            LocalTransform transformForCreatedEntity = LocalTransform.Identity.WithPosition(position);
            ecb.SetComponent(createdEntity, transformForCreatedEntity);

            building.ValueRW.Residents++;
        }
        ecb.Playback(EntityManager);

        bool TryBuy()
        {
            bool haveGold = _duringResources.Gold >= cost.Gold;
            bool haveWood = _duringResources.Gold >= cost.Wood;
            bool haveFood = _duringResources.Gold >= cost.Food;

            if(haveGold && haveWood && haveFood)
            {
                _duringResources.AddValue(Resource.Gold, -cost.Gold);
                _duringResources.AddValue(Resource.Wood, -cost.Wood);
                _duringResources.AddValue(Resource.Food, -cost.Food);
                SystemAPI.SetSingleton(_duringResources);

                return true;
            }
            return false;
        }
    }
}
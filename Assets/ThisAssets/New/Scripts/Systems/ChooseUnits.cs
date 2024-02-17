using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
[UpdateAfter(typeof(InputHandler))]
public partial class ChooseUnits : SystemBase
{
    private CollisionWorld _collisionWorld;
    private InformationAboutControlMode _controlMode;
    private GraphicSettingsComponent _graphicSettings;
    private InformationAboutInputPlayer _input;

    private ChooseUnitsGUI _chooseGUI;

    private float2 _startMousePosition;
    private bool _startOnUIElement;
    private Entity _startEntity;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
        RequireForUpdate<GraphicSettingsComponent>();
        RequireForUpdate<InformationAboutInputPlayer>();
    }
    protected override void OnStartRunning()
    {
        _graphicSettings = SystemAPI.GetSingleton<GraphicSettingsComponent>();
        _chooseGUI = Object.FindObjectOfType<ChooseUnitsGUI>();
    }
    protected override void OnUpdate()
    {
        _collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        _input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();

        float2 duringMousePosition = _input.MousePosition;
        ControlMode mode = _controlMode.ControlMode;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        ClearAllDeadUnits();

        if (_input.ClickDown)
        {
            _startOnUIElement = PointOnScreen.PointOnUIElement(duringMousePosition);
            if (!_startOnUIElement)
            {
                if (mode != ControlMode.Order && mode != ControlMode.Viewing)
                    ClearAllSelectedUnits();

                if (mode != ControlMode.Selection)
                    return;

                _startMousePosition = duringMousePosition;
                _startOnUIElement = PointOnScreen.PointOnUIElement(duringMousePosition);

                if (!_startOnUIElement)
                    SelectUnit();
            }
        }

        if (_input.Hold && mode == ControlMode.Selection && !_startOnUIElement)
        {
            RectInGame duringRect;
            duringRect.Position = _startMousePosition;
            duringRect.Scale = duringMousePosition - _startMousePosition;

            SelectUnitsWithRect(duringRect);
            ChekAllSelectedUnits(duringRect);

            Rect rectForGUI = ConvenatorRectInGame.Convert(duringRect);
            _chooseGUI.TargetRect = rectForGUI;
        }
        else
        {
            _chooseGUI.TargetRect = null;
        }

        if (_input.ClickUp)
            _startOnUIElement = false;

        ecb.Playback(EntityManager);


        #region Methods

        void ClearAllSelectedUnits()
        {
            foreach ((
                RefRO<ChoosedUnit> unit,
                Entity entity) in SystemAPI.Query<
                    RefRO<ChoosedUnit>>().
                    WithEntityAccess())
            {
                ecb.RemoveComponent<ChoosedUnit>(entity);
                _startEntity = Entity.Null;
            }
            foreach ((
                RefRO<GraphicOfChooseMovableUnit> graphic,
                Entity entity) in SystemAPI.Query<
                    RefRO<GraphicOfChooseMovableUnit>>().
                    WithEntityAccess())
            {
                ecb.DestroyEntity(entity);
            }
        }
        void SelectUnit()
        {
            float2 pointOnScreen = _input.MousePosition;

            UnityEngine.Ray rayFromScreen = Camera.main.ScreenPointToRay(new float3(pointOnScreen, 0));
            RaycastInput raycastInput = new RaycastInput()
            {
                Start = rayFromScreen.origin,
                End = rayFromScreen.origin + rayFromScreen.direction * Constants.MouseRange,
                Filter = CollisionFilter.Default
            };
            Unity.Physics.RaycastHit hitInformation;

            _collisionWorld.CastRay(raycastInput, out hitInformation);

            Entity targetEntity = hitInformation.Entity;

            if (!SystemAPI.HasComponent<Unit>(targetEntity) ||
                !SystemAPI.HasComponent<OwnerComponent>(targetEntity))
                return;

            if (SystemAPI.HasComponent<HealthState>(targetEntity))
            {
                HealthState healthState = SystemAPI.GetComponent<HealthState>(targetEntity);
                if (healthState.IsDead)
                    return;
            }
            else
            {
                return;
            }

            OwnerComponent ownerComponent = SystemAPI.GetComponent<OwnerComponent>(targetEntity);

            if (ownerComponent.Owner != OwnersInGame.Player)
            {
                return;
            }
            AddGraphicForEntity(targetEntity);
            ecb.AddComponent<ChoosedUnit>(targetEntity);
            _startEntity = targetEntity;
        }
        void SelectUnitsWithRect(RectInGame targetRect)
        {
            foreach ((
                RefRO<LocalTransform> transform,
                RefRO<Unit> unit,
                RefRO<OwnerComponent> owner,
                RefRO<HealthState> healthState,
                Entity entity) in SystemAPI.Query<
                    RefRO<LocalTransform>,
                    RefRO<Unit>,
                    RefRO<OwnerComponent>,
                    RefRO<HealthState>
                    >().WithEntityAccess())
            {
                if (SystemAPI.HasComponent<ChoosedUnit>(entity))
                    continue;

                if (healthState.ValueRO.IsDead)
                    continue;

                if (owner.ValueRO.Owner != OwnersInGame.Player)
                    continue;

                float3 positionUnitOnScreen = Camera.main.WorldToScreenPoint(transform.ValueRO.Position);

                if (targetRect.Contains(new float2(positionUnitOnScreen.x, positionUnitOnScreen.y)))
                {
                    ecb.AddComponent<ChoosedUnit>(entity);
                    AddGraphicForEntity(entity);
                }
            }
        }
        void ChekAllSelectedUnits(RectInGame targetRect)
        {
            foreach ((
                RefRO<LocalTransform> transform,
                RefRO<ChoosedUnit> choosedUnit,
                Entity entity) in SystemAPI.Query<
                    RefRO<LocalTransform>,
                    RefRO<ChoosedUnit>
                    >().WithEntityAccess())
            {

                float3 positionUnitOnScreen = Camera.main.WorldToScreenPoint(transform.ValueRO.Position);
                if (entity == _startEntity)
                    continue;

                if (!targetRect.Contains(new float2(positionUnitOnScreen.x, positionUnitOnScreen.y)))
                {
                    ecb.RemoveComponent<ChoosedUnit>(entity);
                    foreach ((
                        RefRO<GraphicOfChooseMovableUnit> graphic,
                        Entity entityOfGraphic) in SystemAPI.Query<
                            RefRO<GraphicOfChooseMovableUnit>>().
                            WithEntityAccess())
                    {
                        Parent parentOfGraphic = SystemAPI.GetComponent<Parent>(entityOfGraphic);
                        if (parentOfGraphic.Value != entity)
                            continue;

                        ecb.DestroyEntity(entityOfGraphic);

                    }
                }
            }
        }

        void ClearAllDeadUnits()
        {
            foreach (var (choosedTag, healthState, entity) in SystemAPI.Query<
                RefRO<ChoosedUnit>,
                RefRO<HealthState>>()
                .WithEntityAccess())
            {
                if (!healthState.ValueRO.IsDead)
                    return;

                ecb.RemoveComponent<ChoosedUnit>(entity);
                DynamicBuffer<Child> childs = EntityManager.GetBuffer<Child>(entity);
                foreach (Child child in childs)
                {
                    Entity childEntity = child.Value;
                    if (SystemAPI.HasComponent<GraphicOfChooseMovableUnit>(childEntity))
                        ecb.DestroyEntity(childEntity);
                }
            }
        }

        void AddGraphicForEntity(Entity targetEntity)
        {
            Entity targetGraphic = ecb.Instantiate(_graphicSettings.GraphicOfChooseMovableUnit);
            ecb.AddComponent(targetGraphic, new Parent() { Value = targetEntity });
            ecb.AddComponent<GraphicOfChooseMovableUnit>(targetGraphic);
        }

        #endregion
    }

}
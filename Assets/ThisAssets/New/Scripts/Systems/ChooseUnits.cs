using Unity.Entities;
using UnityEngine.InputSystem;
using Unity.Physics;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ChooseUnits : SystemBase
{
    private CollisionWorld _collisionWorld;
    private InformationAboutControlMode _controlMode;
    private InputAction _inputActionWithClickPosition;
    private InputAction _inputActionWithClick;
    private bool _holding;
    private float2 _startMousePosition;
    private GraphicSettingsComponent _graphicSettings;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
        RequireForUpdate<GraphicSettingsComponent>();
    }
    protected override void OnStartRunning()
    {
        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();

        RuntimePlatform platform = Application.platform;
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            _inputActionWithClick = inputSystem.PC.OnClick;
            _inputActionWithClickPosition = inputSystem.PC.MousePosition;
        }
        _inputActionWithClick.started += (context) => { _holding = true; };
        _inputActionWithClick.canceled += (context) => { _holding = false; };

        _graphicSettings = SystemAPI.GetSingleton<GraphicSettingsComponent>();

    }
    protected override void OnUpdate()
    {
        _collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        float2 duringMousePosition = _inputActionWithClickPosition.ReadValue<Vector2>();
        ControlMode mode = _controlMode.ControlMode;
        if (_inputActionWithClick.triggered)
        {
            if (mode != ControlMode.Move && mode != ControlMode.Viewing)
                ClearAllSelectedUnits();

            if (mode != ControlMode.Selection)
                return;
            _startMousePosition = duringMousePosition;
            SelectUnit();
        }

        if (_holding && mode == ControlMode.Selection)
        {
            RectInGame duringRect;
            duringRect.Position = _startMousePosition;
            duringRect.Scale = duringMousePosition - _startMousePosition;

            SelectUnitsWithRect(duringRect);
            ChekAllSelectedUnits(duringRect);
        }
            

        ecb.Playback(EntityManager);
        void ClearAllSelectedUnits()
        {
            foreach((
                RefRO<ChoosedUnit> unit,
                Entity entity) in SystemAPI.Query<
                    RefRO<ChoosedUnit>>().
                    WithEntityAccess())
            {
                ecb.RemoveComponent<ChoosedUnit>(entity);
            }
            foreach((
                RefRO<GraphicOfChooseUnit> graphic,
                Entity entity) in SystemAPI.Query<
                    RefRO<GraphicOfChooseUnit>>().
                    WithEntityAccess())
            {
                ecb.DestroyEntity(entity);
            }
        }
        void SelectUnit()
        {
            float2 pointOnScreen = _inputActionWithClickPosition.ReadValue<Vector2>();

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

            if (!SystemAPI.HasComponent<Unit>(targetEntity) || !SystemAPI.HasComponent<OwnerComponent>(targetEntity))
                return;

            OwnerComponent ownerComponent = SystemAPI.GetComponent<OwnerComponent>(targetEntity);

            if (ownerComponent.Owner != OwnersInGame.Player)
            {
                return;
            }
            AddGraphicForEntity(targetEntity);
            ecb.AddComponent<ChoosedUnit>(targetEntity);
        }
        void SelectUnitsWithRect(RectInGame targetRect)
        {
            foreach ((
                RefRO<LocalTransform> transform,
                RefRO<Unit> unit,
                RefRO<OwnerComponent> owner,
                Entity entity) in SystemAPI.Query<
                    RefRO<LocalTransform>,
                    RefRO<Unit>,
                    RefRO<OwnerComponent>
                    >().WithEntityAccess())
            {
                if (SystemAPI.HasComponent<ChoosedUnit>(entity))
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
                if (!targetRect.Contains(new float2(positionUnitOnScreen.x, positionUnitOnScreen.y)))
                {
                    ecb.RemoveComponent<ChoosedUnit>(entity);
                    foreach ((
                        RefRO<GraphicOfChooseUnit> graphic,
                        Entity entityOfGraphic) in SystemAPI.Query<
                            RefRO<GraphicOfChooseUnit>>().
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
        void AddGraphicForEntity(Entity targetEntity)
        {
            Entity targetGraphic =  ecb.Instantiate(_graphicSettings.GraphicOfChooseUnit);
            ecb.AddComponent(targetGraphic, new Parent() { Value = targetEntity });
            ecb.AddComponent<GraphicOfChooseUnit>(targetGraphic);
        }
    }
}
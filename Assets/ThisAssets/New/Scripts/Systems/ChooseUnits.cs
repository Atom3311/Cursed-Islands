using Unity.Entities;
using UnityEngine.InputSystem;
using Unity.Physics;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
public partial class ChooseUnits : SystemBase
{
    private CollisionWorld _collisionWorld;
    private InformationAboutControlMode _controlMode;
    private InputAction _inputActionWithClickPosition;
    private InputAction _inputActionWithClick;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
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



    }
    protected override void OnUpdate()
    {
        _collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        if (!_inputActionWithClick.triggered)
            return;

        ControlMode mode = _controlMode.ControlMode;

        if(mode != ControlMode.Move && mode != ControlMode.Viewing)
            ClearAllSelectedUnits();

        if (mode != ControlMode.Selection)
        {
            ecb.Playback(EntityManager);
            return;
        }

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
        {
            ecb.Playback(EntityManager);
            return;
        }

        OwnerComponent ownerComponent = SystemAPI.GetComponent<OwnerComponent>(targetEntity);

        if (ownerComponent.Owner != OwnersInGame.Player)
        {
            ecb.Playback(EntityManager);
            return;
        }

        ecb.AddComponent<ChoosedUnit>(targetEntity);
        ecb.Playback(EntityManager);
        void ClearAllSelectedUnits()
        {
            foreach((RefRO<ChoosedUnit> unit, Entity entity) in SystemAPI.Query<RefRO<ChoosedUnit>>().WithEntityAccess())
            {
                ecb.RemoveComponent<ChoosedUnit>(entity);
            }
        }
    }
}
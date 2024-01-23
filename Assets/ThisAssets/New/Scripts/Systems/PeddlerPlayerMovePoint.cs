using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;
using Physics = Unity.Physics;
using Unity.Physics;
using Unity.Collections;
public partial class PeddlerPlayerMovePoint : SystemBase
{
    private InformationAboutControlMode _controlMode;
    private Physics.CollisionWorld _collisionWorld;
    protected override void OnUpdate()
    {
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        _collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
    }
    protected override void OnCreate()
    {
        RuntimePlatform _platform = Application.platform;
        InputSystem _inputSystem = new InputSystem();
        _inputSystem.Enable();

        Action methodsAfterCheckMode = null;

        if (_platform == RuntimePlatform.WindowsPlayer || _platform == RuntimePlatform.WindowsEditor)
        {
            InputAction _playerMouseAction;
            _playerMouseAction = _inputSystem.PC.MousePosition;
            _inputSystem.PC.OnClik.started += StartWithCheckControlMode;
            methodsAfterCheckMode += PaddlePointWindows;
            void PaddlePointWindows()
            {
                float2 point = _playerMouseAction.ReadValue<Vector2>();
                UnityEngine.Ray rayFromScreen = Camera.main.ScreenPointToRay(new float3(point, 0));
                RaycastInput raycastInput = new RaycastInput()
                {
                    Start = rayFromScreen.origin,
                    End = rayFromScreen.origin + rayFromScreen.direction * Constants.MouseRange,
                    Filter = Physics.CollisionFilter.Default
                };
                NativeList<Physics.RaycastHit> listOfHits = new NativeList<Physics.RaycastHit>(Allocator.Temp);


                _collisionWorld.CastRay(raycastInput, ref listOfHits);

                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

                float3? finalPointForMove = null;

                foreach (Physics.RaycastHit hit in listOfHits)
                {
                    Entity entityOfHit = hit.Entity;
                    if (SystemAPI.HasComponent<SurfaceForMove>(entityOfHit))
                    {
                        finalPointForMove = hit.Position;
                        break;
                    }
                }

                if (!finalPointForMove.HasValue)
                    return;

                foreach ((
                    RefRW<MovePoint> movePoint,
                    RefRO<OwnerComponent> owner) in SystemAPI.Query<
                        RefRW<MovePoint>,
                        RefRO<OwnerComponent>>())
                {
                    if (owner.ValueRO.Owner != OwnersInGame.Player)
                        continue;

                    movePoint.ValueRW.PointInWorld = finalPointForMove.Value;

                }

            }
        }
        void StartWithCheckControlMode(InputAction.CallbackContext context)
        {
            if (_controlMode.ControlMode != TypesOfСontrolModes.Move)
                return;
            methodsAfterCheckMode?.Invoke();
        }
    }

}
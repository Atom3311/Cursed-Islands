using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;
using Physics = Unity.Physics;
using Unity.Physics;
using Unity.Collections;
partial class PeddlerPlayerMovePoint : SystemBase
{
    private InformationAboutControlMode _controlMode;
    private CollisionWorld _collisionWorld;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
    }
    protected override void OnUpdate()
    {
        _collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
    }
    protected override void OnStartRunning()
    {
        RuntimePlatform _platform = Application.platform;
        InputSystem _inputSystem = new InputSystem();
        _inputSystem.Enable();

        Action methodsAfterCheckMode = null;

        if (_platform == RuntimePlatform.WindowsPlayer || _platform == RuntimePlatform.WindowsEditor)
        {
            InputAction _playerMouseAction = _inputSystem.PC.MousePosition;
            _inputSystem.PC.OnClick.started += StartWithCheckControlMode;
            methodsAfterCheckMode += PaddlePointWindows;
            void PaddlePointWindows()
            {
                float2 pointOnScreen = _playerMouseAction.ReadValue<Vector2>();
                if(!PointOnScreen.PointOnUIElement(pointOnScreen))
                    MainPaddleScreenPoint(pointOnScreen);
            }
        }
        else
        {
            InputAction _playerMouseAction = _inputSystem.Android.TapPosition;
            _inputSystem.Android.OnTab.started += StartWithCheckControlMode;
            methodsAfterCheckMode += PaddlePointAndroid;
            void PaddlePointAndroid()
            {
                float2 pointOnScreen = _playerMouseAction.ReadValue<Touch>().position;
                if (!PointOnScreen.PointOnUIElement(pointOnScreen))
                    MainPaddleScreenPoint(pointOnScreen);
            }
        }
        void MainPaddleScreenPoint(float2 pointOnScreen)
        {
            UnityEngine.Ray rayFromScreen = Camera.main.ScreenPointToRay(new float3(pointOnScreen, 0));
            RaycastInput raycastInput = new RaycastInput()
            {
                Start = rayFromScreen.origin,
                End = rayFromScreen.origin + rayFromScreen.direction * Constants.MouseRange,
                Filter = CollisionFilter.Default
            };
            NativeList<Physics.RaycastHit> listOfHits = new NativeList<Physics.RaycastHit>(Allocator.Temp);

            _collisionWorld.CastRay(raycastInput, ref listOfHits);

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

            NativeList<RefRW<MovePoint>> listOfEntities = new NativeList<RefRW<MovePoint>>(Allocator.Temp);

            foreach ((
                RefRW<MovePoint> movePoint,
                RefRO<OwnerComponent> owner,
                RefRO<ChoosedUnit>choosedUnit,
                Entity entity)
                in SystemAPI.Query<
                    RefRW<MovePoint>,
                    RefRO<OwnerComponent>,
                    RefRO<ChoosedUnit>>().WithEntityAccess())
            {
                if (owner.ValueRO.Owner != OwnersInGame.Player)
                    continue;
                listOfEntities.Add(movePoint);
            }

            int countOfEntities = listOfEntities.AsArray().Length;
            foreach (RefRW<MovePoint> movePoint in listOfEntities)
            {
                if (countOfEntities == 1)
                {
                    movePoint.ValueRW.PointInWorld = finalPointForMove.Value;
                }
                else
                {
                    float3 randomPoint = GetRandomPointFromRect.GetRandomPoint(finalPointForMove.Value, math.sqrt(Constants.RectForUnit * countOfEntities));
                    movePoint.ValueRW.PointInWorld = randomPoint;
                }
            }
        }
        void StartWithCheckControlMode(InputAction.CallbackContext context)
        {
            if (_controlMode.ControlMode != ControlMode.Move)
                return;
            methodsAfterCheckMode?.Invoke();
        }
    }
}
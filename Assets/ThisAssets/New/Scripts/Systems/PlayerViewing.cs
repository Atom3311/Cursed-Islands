using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
public partial class PlayerViewing : SystemBase
{
    private InformationAboutControlMode _controlMode;
    private InputAction _inputActionWithHolding;
    private InputAction _inputActionWithClickPosition;
    private bool _holding;
    private float2 _previousPositionClik;
    protected override void OnUpdate()
    {
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        if (!_holding || _controlMode.ControlMode != ControlMode.Viewing)
            return;

        float2 duringMousePosition = _inputActionWithClickPosition.ReadValue<Vector2>();
        float2 translate = _previousPositionClik - duringMousePosition;

        Camera.main.transform.Translate(new float3(translate.x, 0, translate.y) * Constants.SpeedOffsetCameraOnPixel, Space.World);
        _previousPositionClik = duringMousePosition;

    }
    protected override void OnStartRunning()
    {

        RuntimePlatform platform = Application.platform;
        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();

        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            _inputActionWithHolding = inputSystem.PC.OnClick;
            _inputActionWithClickPosition = inputSystem.PC.MousePosition;
        }
        _inputActionWithHolding.started += delegate (InputAction.CallbackContext context)
        {
            _holding = true;
            _previousPositionClik = _inputActionWithClickPosition.ReadValue<Vector2>();
        };
        _inputActionWithHolding.canceled += delegate (InputAction.CallbackContext context)
        {
            _holding = false;
        };
    }
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
    }
}
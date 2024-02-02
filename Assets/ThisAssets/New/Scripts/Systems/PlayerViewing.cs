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
    private RuntimePlatform _platform;
    protected override void OnUpdate()
    {
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        if (!_holding || _controlMode.ControlMode != ControlMode.Viewing)
            return;
        float2 duringMousePosition;
        if (_platform == RuntimePlatform.WindowsEditor || _platform == RuntimePlatform.WindowsPlayer)
            duringMousePosition = _inputActionWithClickPosition.ReadValue<Vector2>();
        else
            duringMousePosition = _inputActionWithClickPosition.ReadValue<Touch>().position;

        float2 translate = (_previousPositionClik - duringMousePosition) * Constants.SpeedOffsetCameraOnPixel;
        Transform cameraTransform = Camera.main.transform;
        cameraTransform.Translate(math.right() * translate.x);
        float3 forward = cameraTransform.forward;
        float3 finalForward = math.normalize(new float3(forward.x, 0, forward.z));
        cameraTransform.Translate(finalForward * translate.y, Space.World);

        _previousPositionClik = duringMousePosition;

    }
    protected override void OnStartRunning()
    {

        _platform = Application.platform;
        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();

        if (_platform == RuntimePlatform.WindowsEditor || _platform == RuntimePlatform.WindowsPlayer)
        {
            _inputActionWithHolding = inputSystem.PC.OnClick;
            _inputActionWithClickPosition = inputSystem.PC.MousePosition;
        }
        else
        {
            _inputActionWithHolding = inputSystem.Android.OnTab;
            _inputActionWithClickPosition = inputSystem.Android.TapPosition;
        }
        _inputActionWithHolding.started += delegate (InputAction.CallbackContext context)
        {
            _holding = true;
            if(_platform == RuntimePlatform.WindowsEditor || _platform == RuntimePlatform.WindowsPlayer)
                _previousPositionClik = _inputActionWithClickPosition.ReadValue<Vector2>();
            else
                _previousPositionClik = _inputActionWithClickPosition.ReadValue<Touch>().position;
        };
        _inputActionWithHolding.canceled += delegate (InputAction.CallbackContext context)
        {
            _holding = false;
        };
    }
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
        Enabled = false;
    }
}
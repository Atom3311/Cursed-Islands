using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
public partial class InputHandler : SystemBase
{
    private bool _platformIsPC;
    private InputAction _inputClick;
    private InputAction _inputClickInformation;
    private bool _isHolding;
    private bool _clickedUp;
    protected override void OnCreate()
    {

        RuntimePlatform platform = Application.platform;
        _platformIsPC = platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer;

        #region StartInputSettings

        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();

        _inputClick = _platformIsPC ?
            inputSystem.PC.OnClick :
            inputSystem.Android.OnTab;

        _inputClickInformation = _platformIsPC ?
            inputSystem.PC.MousePosition :
            inputSystem.Android.TapPosition;

        _inputClick.started += (state) => 
        {
            _isHolding = true;
        };
        _inputClick.canceled += (state) =>
        {
            _isHolding = false;
            _clickedUp = true;
        };

        #endregion

        RequireForUpdate<InformationAboutInputPlayer>();

    }
    protected override void OnUpdate()
    {
        var inputInformation = SystemAPI.GetSingleton<InformationAboutInputPlayer>();

        inputInformation.ClickDown = _inputClick.triggered;
        inputInformation.ClickUp = _clickedUp;
        inputInformation.Hold = _isHolding;
        inputInformation.MousePosition = GetDuringMousePosition();

        SystemAPI.SetSingleton(inputInformation);

        _clickedUp = false;
    }
    private float2 GetDuringMousePosition()
    {
        float2 duringPosition = _platformIsPC ?
            _inputClickInformation.ReadValue<Vector2>() :
            _inputClickInformation.ReadValue<Touch>().position;

        return duringPosition;
    }
}
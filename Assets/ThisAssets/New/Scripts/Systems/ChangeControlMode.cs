using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
public partial class ChangeControlMode : SystemBase
{
    private ControlMode[] _modes;
    private int _duringNumberMode;
    protected override void OnUpdate()
    {
    }
    protected override void OnStartRunning()
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        _duringNumberMode = 0;
        controlMode.ControlMode = _modes[_duringNumberMode];
        SystemAPI.SetSingleton(controlMode);
    }
    protected override void OnCreate()
    {
        _modes = new ControlMode[]
        {
            ControlMode.Viewing,
            ControlMode.Move
        };

        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();   
        RuntimePlatform platform = Application.platform;
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
            inputSystem.PC.ChangeControlMode.started += ChangeMode;

        RequireForUpdate<InformationAboutControlMode>();
    }
    private void ChangeMode(InputAction.CallbackContext context)
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        _duringNumberMode = _duringNumberMode == _modes.Length - 1? 0 : _duringNumberMode + 1;
        controlMode.ControlMode = _modes[_duringNumberMode];
        SystemAPI.SetSingleton(controlMode);
    }
}
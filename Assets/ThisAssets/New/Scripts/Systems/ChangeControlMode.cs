using Unity.Entities;
using UnityEngine;
public partial class ChangeControlMode : SystemBase
{
    private ChooseUnitsGUI _chooseUnitsGUI;
    private TextWithControlMode _textWithControlMode;
    protected override void OnStartRunning()
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        controlMode.ControlMode = ControlMode.Viewing;
        SystemAPI.SetSingleton(controlMode);

        _textWithControlMode.WriteText(ControlMode.Viewing);

        CanChangeControlMode[] _allButtonsWithAbility = Object.FindObjectsByType<CanChangeControlMode>(FindObjectsSortMode.None);
        foreach(CanChangeControlMode button in _allButtonsWithAbility)
            button.ThisButton.onClick.AddListener(() => { ChangeMode(button.TargetMode); });
    }
    protected override void OnCreate()
    {
        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();
        
        RequireForUpdate<InformationAboutControlMode>();

        _chooseUnitsGUI = Object.FindAnyObjectByType<ChooseUnitsGUI>();
        _textWithControlMode = Object.FindAnyObjectByType<TextWithControlMode>();
    }
    private void ChangeMode(ControlMode targetMode)
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        controlMode.ControlMode = targetMode;

        _chooseUnitsGUI.ControlModeIsSelection = targetMode == ControlMode.Selection;
        _textWithControlMode.WriteText(targetMode);
        SystemAPI.SetSingleton(controlMode);
    }
    protected override void OnUpdate()
    {
    }
}
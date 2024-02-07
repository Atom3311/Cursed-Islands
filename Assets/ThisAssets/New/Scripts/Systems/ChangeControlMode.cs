using Unity.Entities;
using UnityEngine;
public partial class ChangeControlMode : SystemBase
{
    private TextWithControlMode _textWithControlMode;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();

        _textWithControlMode = Object.FindAnyObjectByType<TextWithControlMode>();
    }
    protected override void OnStartRunning()
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        controlMode.ControlMode = ControlMode.Viewing;
        SystemAPI.SetSingleton(controlMode);

        _textWithControlMode.WriteText(ControlMode.Viewing);

        CanChangeControlMode[] _allButtonsWithAbility = Object.FindObjectsByType<CanChangeControlMode>(FindObjectsSortMode.None);
        foreach(CanChangeControlMode button in _allButtonsWithAbility)
            button.ThisButton.onClick.AddListener(() => { ChangeMode(button.TargetMode); });

        Enabled = false;
    }
    private void ChangeMode(ControlMode targetMode)
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        controlMode.ControlMode = targetMode;

        _textWithControlMode.WriteText(targetMode);
        SystemAPI.SetSingleton(controlMode);
    }
    protected override void OnUpdate(){}
}
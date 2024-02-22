using Unity.Entities;
using UnityEngine;
public partial class ChangeControlMode : SystemBase
{
    private TextWithControlMode _textWithControlMode;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
    }
    protected override void OnUpdate()
    {
        _textWithControlMode = Object.FindAnyObjectByType<TextWithControlMode>();
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        controlMode.ControlMode = ControlMode.Viewing;
        SystemAPI.SetSingleton(controlMode);

        _textWithControlMode.WriteText(ControlMode.Viewing);

        CanChangeControlMode[] allButtonsWithAbility = Object.FindObjectsByType<CanChangeControlMode>(FindObjectsSortMode.None);

        foreach (CanChangeControlMode button in allButtonsWithAbility)
            button.ThisButton.onClick.AddListener(() => { ChangeMode(button.TargetMode); }
            );

        Enabled = false;
    }
    private void ChangeMode(ControlMode targetMode)
    {
        InformationAboutControlMode controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        controlMode.ControlMode = targetMode;

        _textWithControlMode.WriteText(targetMode);
        SystemAPI.SetSingleton(controlMode);
    }

}
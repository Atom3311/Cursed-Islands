using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
public class ChooseUnitsGUI : MonoBehaviour
{
    public bool ControlModeIsSelection;
    public bool IsHolding;
    public Texture2D TextureForChooseUnits;
    public Color32 ColorForChoose;
    private InputAction _inputActionWithMousePosition;
    private float2 _startMousePosition;
    private RuntimePlatform _platform;
    private void Awake()
    {
        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();
        InputAction inputActionWithClick = null;
        _platform = Application.platform;
        inputActionWithClick = inputSystem.Android.OnTab;
        _inputActionWithMousePosition = inputSystem.Android.TapPosition;
        if (_platform == RuntimePlatform.WindowsEditor || _platform == RuntimePlatform.WindowsPlayer)
        {
            inputActionWithClick = inputSystem.PC.OnClick;
            _inputActionWithMousePosition = inputSystem.PC.MousePosition;
        }
        else
        {
            inputActionWithClick = inputSystem.Android.OnTab;
            _inputActionWithMousePosition = inputSystem.Android.TapPosition;
        }

        inputActionWithClick.started += (context) =>
        {
            IsHolding = true;
            if (_platform == RuntimePlatform.WindowsEditor || _platform == RuntimePlatform.WindowsPlayer)
            {
                _startMousePosition = _inputActionWithMousePosition.ReadValue<Vector2>();
            }
            else
            {
                _startMousePosition = _inputActionWithMousePosition.ReadValue<Touch>().position;
            }
        };

        inputActionWithClick.canceled += (context) => { IsHolding = false; };

    }
    private void OnGUI()
    {
        if (!ControlModeIsSelection || !IsHolding)
            return;
        float2 mousePosition;
        if (_platform == RuntimePlatform.WindowsEditor || _platform == RuntimePlatform.WindowsPlayer)
            mousePosition = _inputActionWithMousePosition.ReadValue<Vector2>();
        else
            mousePosition = _inputActionWithMousePosition.ReadValue<Touch>().position;

        GUI.color = ColorForChoose;
        Rect chooseRect = new Rect(
            _startMousePosition.x,
            Screen.height - _startMousePosition.y,
            mousePosition.x - _startMousePosition.x,
            _startMousePosition.y - mousePosition.y);
        GUI.DrawTexture(chooseRect, TextureForChooseUnits);
    }
}
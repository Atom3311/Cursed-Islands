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
    private void Start()
    {
        InputSystem inputSystem = new InputSystem();
        inputSystem.Enable();
        InputAction inputActionWithClick = null;
        RuntimePlatform platform = Application.platform;
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
        {
            inputActionWithClick = inputSystem.PC.OnClick;
            _inputActionWithMousePosition = inputSystem.PC.MousePosition;
        }

        inputActionWithClick.started += (context) =>
        {
            IsHolding = true;
            _startMousePosition = _inputActionWithMousePosition.ReadValue<Vector2>();
        };

        inputActionWithClick.canceled += (context) => { IsHolding = false; };

    }
    private void OnGUI()
    {
        if (!ControlModeIsSelection || !IsHolding)
            return;
        float2 mousePosition = _inputActionWithMousePosition.ReadValue<Vector2>();
        GUI.color = ColorForChoose;
        Rect chooseRect = new Rect(
            _startMousePosition.x,
            Screen.height - _startMousePosition.y,
            mousePosition.x - _startMousePosition.x,
            _startMousePosition.y - mousePosition.y);
        GUI.DrawTexture(chooseRect, TextureForChooseUnits);
    }
}
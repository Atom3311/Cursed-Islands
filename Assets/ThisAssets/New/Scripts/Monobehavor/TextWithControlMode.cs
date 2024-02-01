using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextWithControlMode : MonoBehaviour
{
    public string StartText;
    private string[] _namesOfControlModes = new string[3];
    private TextMeshProUGUI _textComponent;
    private void Awake()
    {
        _namesOfControlModes[(int)ControlMode.Move] = "Moving";
        _namesOfControlModes[(int)ControlMode.Selection] = "Selection";
        _namesOfControlModes[(int)ControlMode.Viewing] = "Viewing";
        _textComponent = GetComponent<TextMeshProUGUI>();
    }
    public void WriteText(ControlMode mode)
    {
        _textComponent.text = StartText + _namesOfControlModes[(int)mode];
    }
}
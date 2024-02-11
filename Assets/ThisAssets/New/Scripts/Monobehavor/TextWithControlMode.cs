using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextWithControlMode : MonoBehaviour
{
    public string StartText;
    private string[] _namesOfControlModes = new string[3];
    private Text _textComponent;
    private void Awake()
    {
        _namesOfControlModes[(int)ControlMode.Order] = "�������";
        _namesOfControlModes[(int)ControlMode.Selection] = "�����";
        _namesOfControlModes[(int)ControlMode.Viewing] = "��������";
        _textComponent = GetComponent<Text>();
    }
    public void WriteText(ControlMode mode)
    {
        _textComponent.text = StartText + _namesOfControlModes[(int)mode];
    }
}
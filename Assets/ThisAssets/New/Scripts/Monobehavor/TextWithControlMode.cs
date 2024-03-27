using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextWithControlMode : MonoBehaviour
{
    public string StartText;
    private string[] _namesOfControlModes = new string[3];
    private Text _textComponent;
    private Image _imageComponent;
    private Vector4 Color;
    private void Awake()
    {
        _namesOfControlModes[(int)ControlMode.Order] = "Приказы";
        _namesOfControlModes[(int)ControlMode.Selection] = "Выбор";
        _namesOfControlModes[(int)ControlMode.Viewing] = "Просмотр";
        _textComponent = GetComponent<Text>();
        _imageComponent = GetComponent<Image>();
    }
    public void WriteText(ControlMode mode)
    {
        _textComponent.text = StartText + _namesOfControlModes[(int)mode];

        Color = new Vector4(255, 0, 255 , 1);
        _imageComponent.color = Color;
    }
}
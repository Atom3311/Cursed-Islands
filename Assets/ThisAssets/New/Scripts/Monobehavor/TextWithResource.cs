using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextWithResource : MonoBehaviour
{
    public string StartText;
    public Resource TargetResource;
    private Text _textComponent;
    private void Awake()
    {
        _textComponent = GetComponent<Text>();
    }
    public void WriteText(int countOfResource)
    {
        _textComponent.text = StartText + countOfResource;
    }
}
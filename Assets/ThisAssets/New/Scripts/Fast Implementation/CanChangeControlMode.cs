using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class CanChangeControlMode : MonoBehaviour
{
    public Button ThisButton;
    public Image ImageButton;
    public ControlMode TargetMode;
    private void Awake()
    {
        ThisButton = GetComponent<Button>();
        ImageButton = GetComponent<Image>();
    }

    public void Colorrrr()
    {
        ImageButton.color = Color.red;
    }
}

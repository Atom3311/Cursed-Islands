using UnityEngine;
using UnityEngine.UI;
using System;
public class BuyUnitsParametersForUI : MonoBehaviour
{
    public GameObject TargetUI;
    public Button ButtonForClose;
    [HideInInspector] public Button[] ButtonsForSelection;

    [SerializeField] private ButtonForChange[] _buttons;

    public void Init()
    {
        ButtonsForSelection = new Button[_buttons.Length];
        foreach(ButtonForChange button in _buttons)
        {
            ButtonsForSelection[(int)button.Type] = button.Button;
        }
    }

    [Serializable]
    private struct ButtonForChange
    {
        public BuySelection Type;
        public Button Button;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanelOnButtonClick : MonoBehaviour
{
    public static Action<List<string>> OnPanelToOpen;

    [SerializeField] private List<string> panelToOpen = new List<string>();

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonIsPressed);
    }

    private void ButtonIsPressed()
    {
        OnPanelToOpen?.Invoke(panelToOpen);
    }
}
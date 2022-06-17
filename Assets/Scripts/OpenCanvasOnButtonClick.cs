using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OpenCanvasOnButtonClick : MonoBehaviour
{
    public static Action<List<string>> OnCanvasToOpen;
    
    [SerializeField] private List<string> canvasToOpen = new List<string>();

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonIsPressed);
    }

    private void ButtonIsPressed()
    {
        OnCanvasToOpen?.Invoke(canvasToOpen);
    }
}
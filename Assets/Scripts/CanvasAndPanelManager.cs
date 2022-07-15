using System.Collections.Generic;
using UnityEngine;
using System;

public class CanvasAndPanelManager : MonoBehaviour
{
    [SerializeField] private CanvasToShow[] CanvasToOpen;
    [SerializeField] private PanelToShow[] PanelToOpen;

    private void Awake()
    {
        OpenCanvasOnButtonClick.OnCanvasToOpen += ShowCanvas;
        OpenPanelOnButtonClick.OnPanelToOpen += ShowPanel;
    }

    private void ShowCanvas(List<string> canvasNameList)
    {
        foreach (var canvas in CanvasToOpen)
        {
            canvas.CanvasObj.enabled = false;

            if (canvasNameList.Contains(canvas.CanvasName))
            {
                canvas.CanvasObj.enabled = true;
            }
        }
    }

    private void ShowPanel(List<string> panelNameList)
    {
        foreach (var panel in PanelToOpen)
        {
            panel.PanelObj.SetActive(false);

            if (panelNameList.Contains(panel.PanelName))
            {
                panel.PanelObj.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        OpenCanvasOnButtonClick.OnCanvasToOpen -= ShowCanvas;
        OpenPanelOnButtonClick.OnPanelToOpen -= ShowPanel;
    }
}

[Serializable]
public class CanvasToShow
{
    public Canvas CanvasObj;
    public string CanvasName;
}

[Serializable]
public class PanelToShow
{
    public GameObject PanelObj;
    public string PanelName;
}
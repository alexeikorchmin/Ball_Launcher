using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UILevelManager : MonoBehaviour
{
    public static Action OnPlayGame;
    public static Action OnNextLvlButtonPressed;
    public static Action OnResetLvlsButtonPressed;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject lvlResultPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLlvButton;
    [SerializeField] private Button ResetLvlsButton;
    [SerializeField] private TMP_Text lvlResultPanelText;

    private bool noNextLvl = false;

    private void Awake()
    {
        LevelManager.OnLevelComplete += ShowLvlResultPanel;
        LevelManager.OnLevelsFinished += NoNextLvlChangeValue;
        Init();
    }

    private void PlayGame()
    {
        OnPlayGame?.Invoke();
    }

    private void LoadNextLvl()
    {
        OnNextLvlButtonPressed?.Invoke();
    }

    private void ResetLvls()
    {
        OnResetLvlsButtonPressed?.Invoke();
        ResetLvlsButton.gameObject.SetActive(false);
        noNextLvl = false;
    }

    private void ShowLvlResultPanel(bool isLvlComplete)
    {
        if (isLvlComplete)
        {
            lvlResultPanel.SetActive(true);
            lvlResultPanelText.text = "Well done!";
            nextLlvButton.gameObject.SetActive(true);

            if (noNextLvl)
            {
                nextLlvButton.gameObject.SetActive(false);
                ResetLvlsButton.gameObject.SetActive(true);
            }
        }
        else
        {
            lvlResultPanel.SetActive(true);
            lvlResultPanelText.text = "Try again";
            nextLlvButton.gameObject.SetActive(false);
        }
    }

    private void NoNextLvlChangeValue()
    {
        noNextLvl = true;
    }

    private void Init()
    {
        mainMenuPanel.SetActive(true);
        playButton.onClick.AddListener(PlayGame);
        playAgainButton.onClick.AddListener(PlayGame);
        nextLlvButton.onClick.AddListener(LoadNextLvl);
        ResetLvlsButton.onClick.AddListener(ResetLvls);
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= ShowLvlResultPanel;
        LevelManager.OnLevelsFinished -= NoNextLvlChangeValue;
    }
}
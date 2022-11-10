using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILevelManager : MonoBehaviour
{
    public static Action OnPlayGame;
    public static Action OnNextLvlButtonPressed;
    public static Action OnResetLvlsButtonPressed;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject lvlResultPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLvlButton;
    [SerializeField] private Button ResetLvlsButton;
    [SerializeField] private TMP_Text lvlResultPanelText;

    private bool noNextLvl = false;

    private void Awake()
    {
        LevelManager.OnLevelComplete += ShowLvlResultPanel;
        LevelManager.OnLevelsFinished += NoNextLvlChangeValue;
        Init();
    }

    private void Init()
    {
        mainMenuPanel.SetActive(true);
        playButton.onClick.AddListener(PlayGame);
        playAgainButton.onClick.AddListener(PlayGame);
        nextLvlButton.onClick.AddListener(LoadNextLvl);
        ResetLvlsButton.onClick.AddListener(ResetLvls);
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
        noNextLvl = false;
    }

    private void ShowLvlResultPanel(bool isLvlComplete)
    {
        if (isLvlComplete)
        {
            lvlResultPanel.SetActive(true);
            lvlResultPanelText.text = "Well done!";
            nextLvlButton.gameObject.SetActive(true);

            if (noNextLvl)
            {
                nextLvlButton.gameObject.SetActive(false);
                ResetLvlsButton.gameObject.SetActive(true);
            }
        }
        else
        {
            lvlResultPanel.SetActive(true);
            lvlResultPanelText.text = "Try again";
            nextLvlButton.gameObject.SetActive(false);
        }
    }

    private void NoNextLvlChangeValue()
    {
        noNextLvl = true;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= ShowLvlResultPanel;
        LevelManager.OnLevelsFinished -= NoNextLvlChangeValue;
    }
}
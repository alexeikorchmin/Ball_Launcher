using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static Action OnPlayGame;
    public static Action OnNextLvlButtonPressed;
    public static Action OnResetLvlsButtonPressed;

    [SerializeField] private BallLaunchHandler ballLaunchHandler;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject lvlResultPanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLvlButton;
    [SerializeField] private Button resetLvlsButton;
    [SerializeField] private TMP_Text lvlResultPanelText;

    private bool areLevelsFinished = false;

    private void Awake()
    {
        LevelManager.OnLevelComplete += OnLevelCompleteHandler;
        LevelManager.OnLevelsFinished += OnLevelsFinishedHandler;
        Init();
    }

    private void Init()
    {
        mainMenuPanel.SetActive(true);
        playButton.onClick.AddListener(PlayGame);
        playAgainButton.onClick.AddListener(PlayGame);
        nextLvlButton.onClick.AddListener(LoadNextLvl);
        resetLvlsButton.onClick.AddListener(ResetLvls);
        pauseButton.onClick.AddListener(PauseGame);
    }

    private void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        lvlResultPanel.SetActive(false);
        ballLaunchHandler.SetCanTouchBallValue(true);
        pauseButton.interactable = true;
        OnPlayGame?.Invoke();
    }

    private void PauseGame()
    {
        mainMenuPanel.SetActive(true);
        pauseButton.interactable = false;
        ballLaunchHandler.SetCanTouchBallValue(false);
    }

    private void LoadNextLvl()
    {
        lvlResultPanel.SetActive(false);
        ballLaunchHandler.SetCanTouchBallValue(true);
        OnNextLvlButtonPressed?.Invoke();
    }

    private void ResetLvls()
    {
        lvlResultPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        resetLvlsButton.gameObject.SetActive(false);
        ballLaunchHandler.SetCanTouchBallValue(false);
        areLevelsFinished = false;
        OnResetLvlsButtonPressed?.Invoke();
    }

    private void OnLevelCompleteHandler(bool isLvlComplete)
    {
        ballLaunchHandler.SetCanTouchBallValue(false);

        if (isLvlComplete)
        {
            lvlResultPanel.SetActive(true);
            lvlResultPanelText.text = "Well done!";
            nextLvlButton.interactable = true;

            if (areLevelsFinished)
            {
                resetLvlsButton.gameObject.SetActive(true);
                nextLvlButton.interactable = false;
            }
        }
        else
        {
            lvlResultPanel.SetActive(true);
            lvlResultPanelText.text = "Try again";
            nextLvlButton.interactable = false;
        }
    }

    private void OnLevelsFinishedHandler()
    {
        areLevelsFinished = true;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= OnLevelCompleteHandler;
        LevelManager.OnLevelsFinished -= OnLevelsFinishedHandler;
    }
}
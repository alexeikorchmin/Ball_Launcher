using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    public static Action OnNextLevelLoad;
    public static Action OnPlayButtonPressed;

    [SerializeField] private GameObject[] lvlTowers;
    [SerializeField] private GameObject nextLvlPanel;
    [SerializeField] private Button nextLlvButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text lvlPanelText;
    [SerializeField] private int[] targetsPerLvl;
    [SerializeField] private int collisionCountsNeeded = 1;

    private int currentLvl = 0;
    private int totalCollisionCounts = 0;
    private bool isLevelCompleted = false;

    private void Awake()
    {
        TargetColliderHit.OnCollisionCountsComplete += CheckIsLevelComplete;
        BallLaunchHandler.OnBallsOver += ShowPlayAgainPanel;

        nextLlvButton.onClick.AddListener(LoadNextLvl);
        playAgainButton.onClick.AddListener(RestartLevel);
        playButton.onClick.AddListener(PlayButtonPressed);
        StartLevel();
    }

    private void PlayButtonPressed()
    {
        OnPlayButtonPressed?.Invoke();
    }

    private void CheckIsLevelComplete()
    {
        totalCollisionCounts++;

        if (totalCollisionCounts >= collisionCountsNeeded * targetsPerLvl[currentLvl])
        {
            ShowNextLvlPanel();
            isLevelCompleted = true;
        }
    }

    private void ShowNextLvlPanel()
    {
        lvlPanelText.text = "Well done!";
        nextLvlPanel.SetActive(true);
        nextLlvButton.gameObject.SetActive(true);

        if (currentLvl == lvlTowers.Length - 1)
        {
            nextLlvButton.gameObject.SetActive(false);
        }
    }

    private void LoadNextLvl()
    {
        currentLvl++;

        for (int i = 0; i < lvlTowers.Length; i++)
        {
            lvlTowers[i].SetActive(false);

            StartLevel();
            OnNextLevelLoad?.Invoke();
        }
    }

    private void StartLevel()
    {
        lvlTowers[currentLvl].SetActive(true);
        nextLvlPanel.SetActive(false);
        isLevelCompleted = false;
    }

    private void ShowPlayAgainPanel()
    {
        if (!isLevelCompleted)
        {
            ShowNextLvlPanel();
            nextLlvButton.gameObject.SetActive(false);
            lvlPanelText.text = "Don't worry, try again!";
        }
    }

    public void RestartLevel()
    {
        print("game restarted");
    }

    private void OnDestroy()
    {
        BallLaunchHandler.OnBallsOver -= ShowPlayAgainPanel;
        TargetColliderHit.OnCollisionCountsComplete -= CheckIsLevelComplete;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action<bool> OnLevelComplete;
    public static Action OnLevelsFinished;

    [SerializeField] private GameObject[] lvlTowers;
    [SerializeField] private int[] targetsPerLvl;

    private Queue<GameObject> towersQueue = new Queue<GameObject>();
    private int currentLvl = 0;
    private int totalCollisionCounts = 0;
    private bool isLvlComplete = false;

    private void Awake()
    {
        TargetColliderHitHandler.OnTargetCollisionCountsComplete += CheckIsLevelComplete;
        BallLaunchHandler.OnBallsOver += CheckIsLevelFailed;
        UILevelManager.OnNextLvlButtonPressed += LoadNextLvl;
        UILevelManager.OnPlayGame += StartGame;
        UILevelManager.OnResetLvlsButtonPressed += ResetCurrentLvl;
    }

    private void StartGame()
    {
        isLvlComplete = false;
        totalCollisionCounts = 0;

        if (towersQueue.Count > 0)
        {
            Destroy(towersQueue.Dequeue());
        }

        var lvl = Instantiate(lvlTowers[currentLvl]);
        towersQueue.Enqueue(lvl);
    }

    private void CheckIsLevelComplete()
    {
        totalCollisionCounts++;

        if (totalCollisionCounts >= targetsPerLvl[currentLvl])
        {
            isLvlComplete = true;
            OnLevelComplete?.Invoke(isLvlComplete);
        }
    }

    private void CheckIsLevelFailed()
    {
        if (!isLvlComplete)
        {
            OnLevelComplete?.Invoke(isLvlComplete);
        }
    }

    private void LoadNextLvl()
    {
        currentLvl++;

        if (currentLvl < lvlTowers.Length)
        {
            StartGame();
        }

        if (currentLvl == lvlTowers.Length - 1)
        {
            OnLevelsFinished?.Invoke();
        }
    }

    private void ResetCurrentLvl()
    {
        currentLvl = 0;
    }

    private void OnDestroy()
    {
        TargetColliderHitHandler.OnTargetCollisionCountsComplete -= CheckIsLevelComplete;
        BallLaunchHandler.OnBallsOver -= CheckIsLevelFailed;
        UILevelManager.OnNextLvlButtonPressed -= LoadNextLvl;
        UILevelManager.OnPlayGame -= StartGame;
        UILevelManager.OnResetLvlsButtonPressed -= ResetCurrentLvl;
    }
}
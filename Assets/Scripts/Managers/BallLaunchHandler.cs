using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallLaunchHandler : MonoBehaviour
{
    public static Action OnBallsOver;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float detachDelay = 0.15f;

    private List<GameObject> clonePrefabList = new List<GameObject>();
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Collider2D currentBallCollider2D;
    private Camera mainCamera;
    private bool isBallDragging;
    private int ballQuantity = 3;
    private bool canTouch = false;

    private void Awake()
    {
        UILevelManager.OnNextLvlButtonPressed += ResetBalls;
        UILevelManager.OnPlayGame += ResetBalls;
        LevelManager.OnLevelComplete += CanTouchFalse;

        mainCamera = Camera.main;
    }

    private void CanTouchFalse(bool lvlComplete)
    {
        canTouch = false;
    }

    private void ResetBalls()
    {
        ballQuantity = 3;
        canTouch = true;

        DestroyAllBalls();
        SpawnBall();
    }

    private void DestroyAllBalls()
    {
        for (int i = 0; i < clonePrefabList.Count; i++)
        {
            Destroy(clonePrefabList[i]);
        }

        clonePrefabList.Clear();
    }

    private void Update()
    {
        PreLaunchTouchCheck();
    }

    private void PreLaunchTouchCheck()
    {
        if (currentBallRigidbody == null) return;

        if (canTouch == false) return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isBallDragging)
            {
                LaunchBall();
            }

            isBallDragging = false;
            return;
        }

        Vector2 currentTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        if (currentTouchPosition.x == Mathf.Infinity ||
            currentTouchPosition.y == Mathf.Infinity ||
            currentTouchPosition.x > Screen.width / 2)
        {
            currentBallRigidbody.bodyType = RigidbodyType2D.Static;
            return;
        }

        isBallDragging = true;
        currentBallRigidbody.isKinematic = true;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(currentTouchPosition);
        currentBallRigidbody.position = worldPosition;

        if (currentTouchPosition.x > Screen.width / 2 && isBallDragging == true)
        {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        currentBallCollider2D.enabled = true;
        Invoke(nameof(DetachBall), detachDelay);
        isBallDragging = false;
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke(nameof(SpawnBall), spawnDelay);
    }

    private void SpawnBall()
    {
        if (ballQuantity > 0)
        {
            GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
            currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
            currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
            currentBallCollider2D = ballInstance.GetComponent<Collider2D>();
            currentBallCollider2D.enabled = false;
            currentBallSpringJoint.connectedBody = pivot;
            clonePrefabList.Add(ballInstance);
        }
        else
            OnBallsOver?.Invoke();

        ballQuantity--;
    }

    private void OnDestroy()
    {
        UILevelManager.OnNextLvlButtonPressed -= ResetBalls;
        UILevelManager.OnPlayGame -= ResetBalls;
        LevelManager.OnLevelComplete -= CanTouchFalse;
    }
}
using UnityEngine;
using System;

public class TargetColliderHit : MonoBehaviour
{
    public static Action OnTargetCollisionCountsComplete;
    
    private SpriteRenderer colorRenderer;
    private Color defaultColor;
    private int counter;

    private void Awake()
    {
        UILevelManager.OnPlayGame += Init;
        UILevelManager.OnNextLvlButtonPressed += Init;
        colorRenderer = GetComponent<SpriteRenderer>();
        defaultColor = colorRenderer.material.color;
        Init();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        counter++;

        switch (counter)
        {
            case 1:
                colorRenderer.material.color = Color.blue;
                break;
            case 2:
                colorRenderer.material.color = Color.red;
                OnTargetCollisionCountsComplete?.Invoke();
                this.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void Init()
    {
        counter = 0;
        colorRenderer.material.color = defaultColor;
        this.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        UILevelManager.OnPlayGame -= Init;
        UILevelManager.OnNextLvlButtonPressed -= Init;
    }
}
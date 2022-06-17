using UnityEngine;
using System;

public class TargetColliderHit : MonoBehaviour
{
    public static Action OnCollisionCountsComplete;
    
    private SpriteRenderer renderColorHitChanger;
    private int counter = 0;

    private void Awake()
    {
        renderColorHitChanger = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        counter++;

        switch (counter)
        {
            case 1:
                renderColorHitChanger.material.color = Color.blue;
                break;
            case 2:
                renderColorHitChanger.material.color = Color.red;
                OnCollisionCountsComplete?.Invoke();
                counter = 0;
                Destroy(this);
                break;
            default:
                break;
        }
    }
}
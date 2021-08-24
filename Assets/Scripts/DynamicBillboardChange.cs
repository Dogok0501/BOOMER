using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBillboardChange : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    string[] animStates = new string[4] { "Forward", "Backward", "Left", "Right" };
    [SerializeField]
    bool isAnimated;

    Animator anim;
    SpriteRenderer sr;
    EnemyStates enemyStates;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyStates = GetComponent<EnemyStates>();
    }

    private void Update()
    {        
        GetAngle();
    }

    private void GetAngle()
    {
        Vector3 playerDir = Camera.main.transform.forward;
        playerDir.y = 0;
        Vector3 enemyDir = transform.Find("Vision").forward;
        enemyDir.y = 0;

        float dotProduct = Vector2.Dot(playerDir, enemyDir);
        Debug.Log($"dotProduct : {dotProduct}");

        if (dotProduct < -0.5f && dotProduct >= -1.0f)
        {
            ChangeSprite(0);
            Debug.Log("¾Õ");
        }            
        else if (dotProduct > 0.5f && dotProduct <= 1.0f)
        {
            ChangeSprite(1);
            Debug.Log("µÚ");
        }
        else
        {
            Vector3 playerRight = Camera.main.transform.right;
            playerRight.y = 0;
            dotProduct = Vector2.Dot(playerRight, enemyDir);
            if (dotProduct >= 0)
            {
                ChangeSprite(2);
                Debug.Log("¿Þ");
            }
            else
            {
                ChangeSprite(3);
                Debug.Log("¿À");
            }
        }
    }

    void ChangeSprite(int index)
    {
        if (isAnimated)
            anim.Play(animStates[index]);
        else
            sr.sprite = sprites[index];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCrosshair : MonoBehaviour
{
    static public float spread = 0;

    public const double ANIMMING_SPREAD = 0.5;
    public const int JUMP_SPREAD = 50;
    public const int WALK_SPREAD = 20;
    public const int SPRINT_SPREAD = 25;
        
    GameObject topPart;
    GameObject bottomPart;
    GameObject rightPart;
    GameObject leftPart;

    float initialPosition;

    public CanvasGroup canvasGroup;

    private void Start()
    {
        topPart = transform.Find("TopPart").gameObject;
        bottomPart = transform.Find("BottomPart").gameObject;
        rightPart = transform.Find("RightPart").gameObject;
        leftPart = transform.Find("LeftPart").gameObject;

        initialPosition = topPart.GetComponent<RectTransform>().localPosition.y;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(spread != 0)
        {
            topPart.GetComponent<RectTransform>().localPosition = new Vector3(0, initialPosition + spread, 0);
            bottomPart.GetComponent<RectTransform>().localPosition = new Vector3(0, -(initialPosition + spread), 0);
            rightPart.GetComponent<RectTransform>().localPosition = new Vector3(initialPosition + spread, 0, 0);
            leftPart.GetComponent<RectTransform>().localPosition = new Vector3(-(initialPosition + spread), 0, 0);
            spread -= 2f;
        }
    }

    public void HideCrosshair()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowCrosshair()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}

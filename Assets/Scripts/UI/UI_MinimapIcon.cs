using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapIcon : MonoBehaviour
{
    public Transform target;
    public Image icon;
    public RectTransform background;

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && icon != null)
        {
            UI_MinimapUtil.MarkOnTheRPG(target, icon.transform, background, 311, 236);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    Image flashScreen;

    void Start()
    {
        flashScreen = GetComponent<Image>();
    }

    void Update()
    {
        if(flashScreen.color.a > 0)
        {
            Color invisible = new Color(flashScreen.color.r, flashScreen.color.g, flashScreen.color.b, 0);
            flashScreen.color = Color.Lerp(flashScreen.color, invisible, 5 * Time.deltaTime);
        }
    }

    public void TookDamage()
    {
        flashScreen.color = new Color(1f, 0f, 0f, 0.8f);
    }

    public void PickedUpBonus()
    {
        flashScreen.color = new Color(0, 1f, 0f, 0.8f);
    }
}

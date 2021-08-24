using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ammo : MonoBehaviour
{
    public Text ammoText;

    void Start()
    {
        
    }

    public void SetAmmoText(int ammo, int clip)
    {
        ammoText.text = ammo + " / " + clip ;
    }
}

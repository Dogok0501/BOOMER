using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public float damage;
    public int maxAmmo; // ÃÖ´ë ¼ÒÁö Åº¾à
    public int maxClip; // ÅºÃ¢ »çÀÌÁî
    public int pellets;
    public float bulletSpread;
    public float range;
    public float firerate;
    public float recoil;
    public float kickback;
    public float aimSpeed;
    public float reloadTime;
    public float drawSpeed;
    public bool burst; // ¿¬»ç
    public bool boltAction; // ³ë¸®¼è ÈÄÅð ÀüÁø
    public GameObject prefab;

    private int currentAmmo; // ÇöÀç º¸À¯ Åº¾à
    private int currentClip; // ÇöÀç ÅºÃ¢ ³» Åº¾à

    public void Initialize()
    {
        currentAmmo = maxAmmo / 4;
        currentClip = maxClip;
    }

    public bool CanFireBullet()
    {
        if(currentClip > 0)
        {
            currentClip--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reload()
    {
        currentAmmo += currentClip;
        currentClip = Mathf.Min(maxClip, currentAmmo);
        currentAmmo -= currentClip;
    }

    public void ObtainAmmo(int ammo)
    {
        if (currentAmmo + ammo >= maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo += ammo;
        }        
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetCurrentClip()
    {
        return currentClip;
    }    
}

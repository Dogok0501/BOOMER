using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float maxArmor = 150;
    public FlashScreen flash;

    public float currentArmor;
    public float currentHealth;

    UI_PlayerHealthArmor healtharmorBar;

    UI_Upgrade upgrade;

    void Awake()
    {
        healtharmorBar = GameObject.Find("Health and Armor").GetComponent<UI_PlayerHealthArmor>();
        upgrade = GameObject.Find("Upgrade Panel").GetComponent<UI_Upgrade>();
    }

    void Start()
    {       
        currentHealth = maxHealth;
        currentArmor = maxArmor;
                
        healtharmorBar.SetMaxHealth(maxHealth);
        healtharmorBar.SetMaxArmor(maxArmor);

        upgrade.SetHealthSlider(maxHealth);
        upgrade.SetArmorSlider(maxArmor);
    }

    private void Update()
    {
        currentArmor = Mathf.Clamp(currentArmor, -Mathf.Infinity, maxArmor);
        currentHealth = Mathf.Clamp(currentHealth, -Mathf.Infinity, maxHealth);

        if(currentHealth <= 0)
        {
            Managers.Clear();
            Managers.Scene.LoadScene(SceneType.Scene.GameOver);
        }


    }

    public void AddHealth(float value) 
    {
        currentHealth += value;

        healtharmorBar.SetHealth(currentHealth);
    }

    public void AddArmor(float value)
    {
        currentArmor += value;

        healtharmorBar.SetArmor(currentArmor);
    }

    private void HitByEnemy (float damage)
    {
        if(currentArmor > 0 && currentArmor >= damage)
        {
            currentArmor -= damage;

            healtharmorBar.SetArmor(currentArmor);
        }
        else if(currentArmor > 0 && currentArmor < damage)
        {
            damage -= currentArmor;
            currentArmor = 0;
            currentHealth -= damage;

            healtharmorBar.SetHealth(currentHealth);
        }
        else
        {
            currentHealth -= damage;

            healtharmorBar.SetHealth(currentHealth);
        }

        Managers.Sound.Play("playerHit", SoundManager.SoundType.Effect);
        flash.TookDamage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealthArmor : MonoBehaviour
{
    public Slider healthSlider;
    public Image healthFill;
    public Text healthText;

    public Slider armorSlider;
    public Image armorFill;
    public Text armorText;

    public Gradient gradient;

    public void Start()
    {
        //healthSlider = Util.FindChild<Slider>(this.gameObject, "Health Bar", true);//GameObject.Find("Health Bar").GetComponent<Slider>();
        //healthFill = Util.FindChild<Image>(healthSlider.gameObject, "Current Health", true);

        //armorSlider = Util.FindChild<Slider>(this.gameObject, "Armor Bar", true);//GameObject.Find("Armor Bar").GetComponent<Slider>();
        //armorFill = Util.FindChild<Image>(armorSlider.gameObject, "Current Armor", true);
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        healthFill.color = gradient.Evaluate(1f);

        healthText.text = string.Format("{0}/{1}", health, healthSlider.maxValue);
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
        healthFill.color = gradient.Evaluate(healthSlider.normalizedValue);

        if (health > healthSlider.maxValue)
        {
            health = healthSlider.maxValue;
        }

        healthText.text = string.Format("{0}/{1}", health, healthSlider.maxValue);
    }

    public void SetMaxArmor(float armor)
    {
        armorSlider.maxValue = armor;
        armorSlider.value = armor;

        armorFill.color = gradient.Evaluate(1f);

        armorText.text = string.Format("{0}/{1}", armor, armorSlider.maxValue);
    }

    public void SetArmor(float armor)
    {
        armorSlider.value = armor;
        armorFill.color = gradient.Evaluate(armorSlider.normalizedValue);

        if (armor > armorSlider.maxValue)
        {
            armor = armorSlider.maxValue;
        }

        armorText.text = string.Format("{0}/{1}", armor, armorSlider.maxValue);
    }
}

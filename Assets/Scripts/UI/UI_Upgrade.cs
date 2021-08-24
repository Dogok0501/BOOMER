using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade : MonoBehaviour
{
    [Header("Health")]
    public Slider healthUpgradeSlider;
    public Image healthUpgradeFill;
    public Button healthUpgradeButton;
    public Text healthUpgradeText;

    [Header("Armor")]
    public Slider armorUpgradeSlider;
    public Image armorUpgradeFill;
    public Button armorUpgradeButton;
    public Text armorUpgradeText;

    [Header("Shockwave")]
    public Slider shockwaveUpgradeSlider;
    public Image shockwaveUpgradeFill;
    public Button shockwaveUpgradeButton;
    public Text shockwaveUpgradeText;

    [Header("Engram Text")]
    public Text engramText;
    public Text healthEngramNeedText;
    public Text armorEngramNeedText;
    public Text shockwaveEngramNeedText;
        
    [Header("Upgrade Data")]
    public float healthMaxUpgrade;
    public float armorMaxUpgrade;
    public float shockwaveMaxUpgrade;

    [Space (10f)]
    public int healthUpgradeEngramNeed;
    public int armorUpgradeEngramNeed;
    public int shockwaveUpgradeEngramNeed;

    [Header("Component")]
    PlayerHealth playerHealth;
    UI_PlayerHealthArmor healtharmorBar;
    Inventory inventory;
    PlayerSkillController playerSkillController;

    private void Awake()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        healtharmorBar = GameObject.Find("Health and Armor").GetComponent<UI_PlayerHealthArmor>();
        inventory = Util.FindChild<Inventory>(GameObject.FindGameObjectWithTag("Player"), null, true);
        playerSkillController = GameObject.Find("Player").GetComponent<PlayerSkillController>();
    }

    void Start()
    { 
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
        armorUpgradeButton.onClick.AddListener(UpgradeArmor);
        shockwaveUpgradeButton.onClick.AddListener(UpgradeShockwave);

        healthEngramNeedText.text = string.Format("X {0}", healthUpgradeEngramNeed.ToString());
        armorEngramNeedText.text = string.Format("X {0}", armorUpgradeEngramNeed.ToString());
        shockwaveEngramNeedText.text = string.Format("X {0}", shockwaveUpgradeEngramNeed.ToString());
    }

    void Update()
    {
        engramText.text = string.Format("X {0}", EngramCount().ToString());        
    }

    #region health

    public void SetHealthSlider(float health)
    {
        healthUpgradeSlider.minValue = health;
        healthUpgradeSlider.value = health;
        healthUpgradeSlider.maxValue = healthMaxUpgrade;

        healthUpgradeText.text = string.Format("{0}/{1}", healthUpgradeSlider.value, healthUpgradeSlider.maxValue);
    }

    public void UpgradeHealth()
    {
        if(EngramCount() >= healthUpgradeEngramNeed)
        {
            if (healthUpgradeSlider.value >= healthUpgradeSlider.maxValue)
            {
                SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Upgrade Full!"));
            }
            else
            {
                for (int i = 0; i < healthUpgradeEngramNeed; i++)
                {
                    inventory.SubItem(1003);
                }

                healthUpgradeSlider.value += healthUpgradeSlider.maxValue / 20;
                healthUpgradeText.text = string.Format("{0}/{1}", healthUpgradeSlider.value, healthUpgradeSlider.maxValue);

                playerHealth.maxHealth = healthUpgradeSlider.value;
                playerHealth.currentHealth = healthUpgradeSlider.value;
                healtharmorBar.SetMaxHealth(healthUpgradeSlider.value);

                healthUpgradeEngramNeed = healthUpgradeEngramNeed * 2;
                healthEngramNeedText.text = string.Format("X {0}", healthUpgradeEngramNeed.ToString());
            }                  
        }
        else
        {
            SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Not enough Engram!"));
        }
    }

    #endregion

    #region armor

    public void SetArmorSlider(float armor)
    {
        armorUpgradeSlider.minValue = armor;
        armorUpgradeSlider.value = armor;
        armorUpgradeSlider.maxValue = armorMaxUpgrade;

        armorUpgradeText.text = string.Format("{0}/{1}", armorUpgradeSlider.value, armorUpgradeSlider.maxValue);
    }

    public void UpgradeArmor()
    {
        if (EngramCount() >= armorUpgradeEngramNeed)
        {
            if (armorUpgradeSlider.value >= armorUpgradeSlider.maxValue)
            {
                SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Upgrade Full!"));
            }
            else
            {
                for (int i = 0; i < armorUpgradeEngramNeed; i++)
                {
                    inventory.SubItem(1003);
                }

                armorUpgradeSlider.value += armorUpgradeSlider.maxValue / 20;
                armorUpgradeText.text = string.Format("{0}/{1}", armorUpgradeSlider.value, armorUpgradeSlider.maxValue);

                playerHealth.maxArmor = armorUpgradeSlider.value;
                playerHealth.currentArmor = armorUpgradeSlider.value;
                healtharmorBar.SetMaxArmor(armorUpgradeSlider.value);

                armorUpgradeEngramNeed = armorUpgradeEngramNeed * 2;
                armorEngramNeedText.text = string.Format("X {0}", armorUpgradeEngramNeed.ToString());
            }            
        }
        else
        {
            SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Not enough Engram!"));
        }
    }

    #endregion

    #region shockwave

    public void SetShockwaveSlider(float damage)
    {
        shockwaveUpgradeSlider.minValue = damage;
        shockwaveUpgradeSlider.value = damage;
        shockwaveUpgradeSlider.maxValue = shockwaveMaxUpgrade;

        shockwaveUpgradeText.text = string.Format("{0}/{1}", shockwaveUpgradeSlider.value, shockwaveUpgradeSlider.maxValue);
    }

    public void UpgradeShockwave()
    {
        if (EngramCount() >= shockwaveUpgradeEngramNeed)
        {
            if (shockwaveUpgradeSlider.value >= shockwaveUpgradeSlider.maxValue)
            {
                SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Upgrade Full!"));
            }
            else
            {
                for (int i = 0; i < shockwaveUpgradeEngramNeed; i++)
                {
                    inventory.SubItem(1003);
                }

                shockwaveUpgradeSlider.value += shockwaveUpgradeSlider.maxValue / 20;
                shockwaveUpgradeText.text = string.Format("{0}/{1}", shockwaveUpgradeSlider.value, shockwaveUpgradeSlider.maxValue);

                playerSkillController.ShockwaveDamageSetter = shockwaveUpgradeSlider.value;

                shockwaveUpgradeEngramNeed = shockwaveUpgradeEngramNeed * 2;
                shockwaveEngramNeedText.text = string.Format("X {0}", shockwaveUpgradeEngramNeed.ToString());
            }
        }
        else
        {
            SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Not enough Engram!"));
        }
    }

    #endregion    

    int EngramCount()
    {
        int engramAmount = 0;

        for (int j = 0; j < inventory.slotAmount; j++)
        {
            if (inventory.items[j].index == 1003)
            {
                ItemData engram = inventory.slots[j].transform.GetChild(0).GetComponent<ItemData>();
                engramAmount = engram.amount;
                return engramAmount;
            }
        }
        return 0;
    }
}

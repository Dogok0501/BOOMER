using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : MonoBehaviour
{
    PlayerSkillController playerSkillController;

    public Image skillGrenade;
    public Image skillShockwave;

    // Start is called before the first frame update
    void Start()
    {
        playerSkillController = GameObject.Find("Player").GetComponent<PlayerSkillController>();
        skillGrenade = GameObject.Find("Skill_Grenade").GetComponent<Image>();
        skillShockwave = GameObject.Find("Skill_Shockwave").GetComponent<Image>();

        skillGrenade.fillAmount = 0;
        skillShockwave.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerSkillController.isGrenadeCooldown)
        {            
            skillGrenade.fillAmount -= 1 / playerSkillController.grenadeCooltime * Time.deltaTime;

            if(skillGrenade.fillAmount <= 0)
            {
                skillGrenade.fillAmount = 0;
                playerSkillController.isGrenadeCooldown = false;
            }
        }

        if (playerSkillController.isShockwaveCooldown)
        {
            skillShockwave.fillAmount -= 1 / playerSkillController.shockwaveCooltime * Time.deltaTime;

            if (skillShockwave.fillAmount <= 0)
            {
                skillShockwave.fillAmount = 0;
                playerSkillController.isShockwaveCooldown = false;
            }
        }
    }
}

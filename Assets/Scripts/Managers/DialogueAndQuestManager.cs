using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAndQuestManager
{
    // 대화창에 필요한 변수들
    GameObject dialoguePanel;
    public NPC NPCToAdd;
    public NPCDialogue dialogueToAdd;
    public NPCQuest questToGenerate;
    public ConsumableItem rewardItem;
    public Button nextButton;
    public Button acceptButton;
    public Button upgradeButton;
    public Sprite itemSprite;

    GameObject NPCQuestPanel;

    GameObject dialoguePaneltoUpgrade;
    GameObject NPCUpgradePanel;

    GameObject HUD;

    Quest quest;

    public void Init()
    {   
        quest = GameObject.Find("InventoryThings").GetComponent<Quest>();

        dialoguePanel = GameObject.Find("Dialogue Panel");
        NPCQuestPanel = GameObject.Find("NPC Quest Panel");
        dialoguePaneltoUpgrade = GameObject.Find("Dialogue Panel to Upgrade");
        NPCUpgradePanel = GameObject.Find("Upgrade Panel");
        HUD = GameObject.Find("HUD");

        dialoguePanel.SetActive(false);
        NPCQuestPanel.SetActive(false);
        dialoguePaneltoUpgrade.SetActive(false);
        NPCUpgradePanel.SetActive(false);

        nextButton = Util.FindChild<Button>(dialoguePanel, "Quest Button");
        nextButton.onClick.AddListener(GenerateQuest);

        acceptButton = Util.FindChild<Button>(NPCQuestPanel, "Accept Button");
        acceptButton.onClick.AddListener(AcceptQuest);

        upgradeButton = Util.FindChild<Button>(dialoguePaneltoUpgrade, "Upgrade Button");
        upgradeButton.onClick.AddListener(GenerateUpgrade);
    }
       
    public void GenerateDialogue(int index)
    {
        //데이터 넣어주기
        NPCToAdd = Managers.Data.npcDict[index];
        dialogueToAdd = Managers.Data.npcDialogueDict[NPCToAdd.dialogueindex];
        if (index == 2002)
        {
            dialoguePaneltoUpgrade.transform.GetChild(0).GetComponent<Text>().text = NPCToAdd.name;
            dialoguePaneltoUpgrade.transform.GetChild(1).GetComponent<Text>().text = dialogueToAdd.dialogue_start;
        }
        else
        {
            dialoguePanel.transform.GetChild(0).GetComponent<Text>().text = NPCToAdd.name;
            dialoguePanel.transform.GetChild(1).GetComponent<Text>().text = dialogueToAdd.dialogue_start;
        }            

        // 대화창 활성화 및 구동 
        Managers.Scene.Pause();        
        HUD.SetActive(false);
        if(index == 2002)
        {
            dialoguePaneltoUpgrade.SetActive(true);
        }
        else
        {
            dialoguePanel.SetActive(true);
        }
    }

    public void GenerateQuest()
    {        
        questToGenerate = Managers.Data.npcQuestDict[NPCToAdd.questindex];
        rewardItem = Managers.Data.consumableItemDict[questToGenerate.rewarditemindex];

        dialoguePanel.transform.GetChild(1).GetComponent<Text>().text = dialogueToAdd.dialogue_progress;

        NPCQuestPanel.SetActive(true);

        NPCQuestPanel.transform.Find("Script Panel").GetChild(0).GetComponent<Text>().text = questToGenerate.description;

        itemSprite =  Resources.Load<Sprite>($"Graphics/Bonuses/{rewardItem.sprite}");
        NPCQuestPanel.transform.Find("Reward Panel").GetChild(0).GetComponent<Image>().sprite = itemSprite;
        NPCQuestPanel.transform.Find("Reward Panel").GetChild(1).GetComponent<Text>().text = rewardItem.name;

        acceptButton.interactable = true;
    }

    public void AcceptQuest()
    {
        quest.AddQuest(NPCToAdd.questindex);

        NPCQuest questToAdd = Managers.Data.npcQuestDict[NPCToAdd.questindex];

        if (quest.CheckQuestExist(questToAdd) == true)
        {
            acceptButton.interactable = false;
            dialoguePanel.transform.GetChild(1).GetComponent<Text>().text = dialogueToAdd.dialogue_end1;
        }            
    }

    public void GenerateUpgrade()
    {
        NPCUpgradePanel.SetActive(true);
        dialoguePaneltoUpgrade.transform.GetChild(1).GetComponent<Text>().text = dialogueToAdd.dialogue_progress;
    }

    public void EndDialogue()
    {
        Managers.Scene.Resume();
        dialoguePanel.SetActive(false);
        NPCQuestPanel.SetActive(false);
        dialoguePaneltoUpgrade.SetActive(false);
        NPCUpgradePanel.SetActive(false);
        HUD.SetActive(true);
        GameObject.Find("HUD").SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerLook>().enabled = true;
    }
}

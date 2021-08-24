using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{    
    GameObject questPanel;
    Inventory inventory;

    public GameObject questSlot;
    public GameObject questText;

    public Dictionary<int, NPCQuest> questList = new Dictionary<int, NPCQuest>();
    public List<GameObject> slots = new List<GameObject>();

    int questAmount;
    string data;

    int[] huntGoalCount;

    void Start()
    {
        inventory = GetComponent<Inventory>();

        questAmount = 5;
        huntGoalCount = new int[questAmount];
        questPanel = GameObject.Find("Player Quest Panel");
        
        for (int i = 0; i < questAmount; i++)
        {
            questList.Add(i, new NPCQuest());
            huntGoalCount[i] = 0;
            slots.Add(Instantiate(questSlot));
            slots[i].transform.SetParent(questPanel.transform);
        }

        questPanel.SetActive(false);
        AddQuest(2102);
    }

    void Update()
    {
        RefreshGoal();

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!questPanel.activeSelf)
            {
                questPanel.SetActive(true);
            }
            else if (questPanel.activeSelf)
            {
                questPanel.SetActive(false);
            }
        }
    }

    void RefreshGoal()
    {
        for (int i = 0; i < questAmount; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                return;                
            }
            else if (questList[i].type == "Hunt")
            {
                data = "<color=#FFFFFF><b>" + questList[i].name + "</b></color>\n" + $"{huntGoalCount[i]} / {questList[i].goal}" + "";
                slots[i].transform.GetChild(0).GetComponent<Text>().text = data;

                if(huntGoalCount[i] == questList[i].goal)
                {
                    SubQuest(questList[i].index);
                    break;
                }
            }
            else if (questList[i].type == "Collect")
            {
                int collectGoalCount = 0;

                for (int j = 0; j < inventory.slotAmount; j++)
                {
                    if (inventory.items[j].index == questList[i].targetindex)
                    {
                        ItemData itemData = inventory.slots[j].transform.GetChild(0).GetComponent<ItemData>();
                        collectGoalCount = itemData.amount;

                        if (collectGoalCount == questList[i].goal)
                        {
                            inventory.AddItem(questList[i].rewarditemindex);

                            for (int k = 0; k < questList[i].goal; k++)
                            {
                                inventory.SubItem(questList[i].targetindex);
                            }
                            
                            SubQuest(questList[i].index);
                            break;
                        }
                    }
                }

                data = "<color=#FFFFFF><b>" + questList[i].name + "</b></color>\n" + $"{collectGoalCount} / {questList[i].goal}" + "";
                slots[i].transform.GetChild(0).GetComponent<Text>().text = data;                                
            }
        }
    }

    void HuntCounter(int index)
    {
        for (int i = 0; i < questAmount; i++)
        {
            if (index == questList[i].targetindex)
            {
                Debug.Log(index);
                huntGoalCount[i]++;
            }                
        }
    }

    public void AddQuest(int id)
    {
        NPCQuest questToAdd = Managers.Data.npcQuestDict[id];

        questPanel.SetActive(true);

        if (CheckQuestExist(questToAdd))
        {
            for (int i = 0; i < questList.Count; i++)
            {
                if (questList[i].index == id)
                {
                    Debug.Log("Quest already exist");
                    SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate("Quest already exist"));
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < questList.Count; i++)
            {
                if (questList[i].index == -1)
                {
                    questList[i] = questToAdd;
                    GameObject questObj = Instantiate(questText);
                    questObj.transform.SetParent(slots[i].transform);
                    questObj.transform.position = slots[i].transform.position;

                    SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate($"Quest : {questList[i].name} Obtain"));
                    break;
                }
            }
        }
    }

    public void SubQuest(int id)
    {
        NPCQuest questToSub = Managers.Data.npcQuestDict[id];

        if (CheckQuestExist(questToSub))
        {
            for (int i = 0; i < questList.Count; i++)
            {
                if (questList[i].index == id)
                {
                    SystemMessage.instance.StartCoroutine(SystemMessage.instance.TextUpdate($"Quest : {questList[i].name} Complete"));
                    if(questList[i].type == "Hunt")
                    {
                        huntGoalCount[i] = 0;
                    }
                    questList[i] = new NPCQuest();
                    GameObject questObj = slots[i].transform.GetChild(0).gameObject;
                    Destroy(questObj);                    

                    Invoke("SortQuestList", 0.25f);
                    break;
                }
            }
        }
    }

    public bool CheckQuestExist(NPCQuest quest)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].index == quest.index)
                return true;
        }
        return false;
    }    

    void SortQuestList()
    {
        List<NPCQuest> tempList = new List<NPCQuest>();
        List<int> tempGoal = new List<int>();

        for (int i = 0; i < questAmount; i++)
        {
            if(questList[i].index != -1)
            {
                if (questList[i].type == "Hunt")
                {
                    tempList.Add(questList[i]);
                    tempGoal.Add(huntGoalCount[i]);
                    GameObject questObj = slots[i].transform.GetChild(0).gameObject;
                    Destroy(questObj);
                    questList[i] = new NPCQuest();
                }
                else if (questList[i].type == "Collect")
                {
                    tempList.Add(questList[i]);
                    tempGoal.Add(0);
                    GameObject questObj = slots[i].transform.GetChild(0).gameObject;
                    Destroy(questObj);
                    questList[i] = new NPCQuest();
                }                
            }
        }

        for (int k = 0; k < tempList.Count; k++)
        {
            if(questList[k].index == -1)
            {
                if (tempList[k].type == "Hunt")
                {
                    questList[k] = tempList[k];
                    huntGoalCount[k] = tempGoal[k];
                    GameObject questObj = Instantiate(questText);
                    questObj.transform.SetParent(slots[k].transform);
                    questObj.transform.position = slots[k].transform.position;
                }
                else if (tempList[k].type == "Collect")
                {
                    questList[k] = tempList[k];
                    GameObject questObj = Instantiate(questText);
                    questObj.transform.SetParent(slots[k].transform);
                    questObj.transform.position = slots[k].transform.position;
                }
            }
        }
        tempGoal.Clear();
        tempList.Clear();
    }
}

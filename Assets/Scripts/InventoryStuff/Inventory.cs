using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject inventoryPanel;
    GameObject slotPanel;
    Sprite itemSprite;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public int slotAmount;

    public Dictionary<int, ConsumableItem> items = new Dictionary<int, ConsumableItem>();
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        slotAmount = 36;
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
        for(int i = 0; i < slotAmount; i++)
        {
            items.Add(i, new ConsumableItem());                 // 모든 아이템 Dictionary에 빈 -1의 index값을 부여함으로 빈 아이템으로 만듬
            slots.Add(Instantiate(inventorySlot));              // 모든 인벤토리 슬롯 List에 인벤토리 슬롯 프리팹을 생성
            slots[i].GetComponent<InventorySlot>().slot_slotID = i;  // i번째 slot에는 각각 i라는 id값을 InventorySlot의 slot_slotID에 전달, itmes[여기값]과 동일한 값
            slots[i].transform.SetParent(slotPanel.transform);  // 생성된 slot들을 slotPanel을 부모로 설정함으로 slotPanel의 Grid Layout Group로 설정해놓은 위치로 정렬됨.
        }

        inventoryPanel.SetActive(false);
                        
        AddItem(1000);
        AddItem(1003);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryPanel.activeSelf && !Managers.gameIsPaused)
            {
                Managers.Scene.Pause();
                inventoryPanel.SetActive(true);
            }
            else if (inventoryPanel.activeSelf && Managers.gameIsPaused)
            {
                Managers.Scene.Resume();
                inventoryPanel.SetActive(false);
            }
        }
        
    }

    public void AddItem(int id)
    {
        ConsumableItem itemToAdd = Managers.Data.consumableItemDict[id]; // consumableItemDict에서 key 값으로 찾은 value값을 저장.
        if (itemToAdd.stackable && CheckItemExist(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].index == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++) // 좌측 상단 슬롯부터 차례대로 검사
            {
                if (items[i].index == -1)
                {
                    items[i] = itemToAdd;

                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd; // ItemData에 해당 아이템 Dictionary value 정보를 전달
                    itemObj.GetComponent<ItemData>().amount = 1;
                    itemObj.GetComponent<ItemData>().item_slotID = i;  // ItemData에 해당 아이템이 들어간 item_slotID의 위치를 전달
                    itemObj.transform.SetParent(slots[i].transform);
                    itemSprite = Resources.Load<Sprite>($"Graphics/Bonuses/{itemToAdd.sprite}");
                    itemObj.transform.position = slots[i].transform.position;
                    itemObj.GetComponent<Image>().sprite = itemSprite;

                    break;
                }
            }
        }
    }

    public void SubItem(int id)
    {
        ConsumableItem itemToSub = Managers.Data.consumableItemDict[id];
        if (CheckItemExist(itemToSub))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].index == id && slots[i].transform.GetChild(0).GetComponent<ItemData>().amount >= 2)
                {
                    GameObject itemObj = slots[i].transform.GetChild(0).gameObject;
                    ItemData data = itemObj.GetComponent<ItemData>();
                    data.amount--;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
                else if (items[i].index == id && slots[i].transform.GetChild(0).GetComponent<ItemData>().amount == 1)
                {
                    Destroy(slots[i].transform.GetChild(0).gameObject);
                    items[i] = new ConsumableItem();

                    break;
                }
            }
        }

    }

    bool CheckItemExist(ConsumableItem item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].index == item.index)
                return true;
        }
        return false;
    }

    void Pause()
    {
        inventoryPanel.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerLook>().enabled = false;
        GameObject.Find("Player").GetComponent<Weapon>().enabled = false;
        Time.timeScale = 0f;
        Managers.gameIsPaused = true;
    }

    void Resume()
    {        
        inventoryPanel.SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerLook>().enabled = true;
        GameObject.Find("Player").GetComponent<Weapon>().enabled = true;
        GameObject.Find("Player").SetActive(true);
        Time.timeScale = 1f;
        Managers.gameIsPaused = false;
    }    
}

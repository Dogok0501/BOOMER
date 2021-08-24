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
            items.Add(i, new ConsumableItem());                 // ��� ������ Dictionary�� �� -1�� index���� �ο������� �� ���������� ����
            slots.Add(Instantiate(inventorySlot));              // ��� �κ��丮 ���� List�� �κ��丮 ���� �������� ����
            slots[i].GetComponent<InventorySlot>().slot_slotID = i;  // i��° slot���� ���� i��� id���� InventorySlot�� slot_slotID�� ����, itmes[���Ⱚ]�� ������ ��
            slots[i].transform.SetParent(slotPanel.transform);  // ������ slot���� slotPanel�� �θ�� ���������� slotPanel�� Grid Layout Group�� �����س��� ��ġ�� ���ĵ�.
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
        ConsumableItem itemToAdd = Managers.Data.consumableItemDict[id]; // consumableItemDict���� key ������ ã�� value���� ����.
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
            for (int i = 0; i < items.Count; i++) // ���� ��� ���Ժ��� ���ʴ�� �˻�
            {
                if (items[i].index == -1)
                {
                    items[i] = itemToAdd;

                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd; // ItemData�� �ش� ������ Dictionary value ������ ����
                    itemObj.GetComponent<ItemData>().amount = 1;
                    itemObj.GetComponent<ItemData>().item_slotID = i;  // ItemData�� �ش� �������� �� item_slotID�� ��ġ�� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI Slot 프리팹에 붙는 스크립트
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int slot_slotID; // Inventory class line.28
    Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("InventoryThings").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>(); // 이동시키려는 아이템
        if (inventory.items[slot_slotID].index == -1) // droppedItem이 이사갈 자리에 이미 있는 아이템이 빈 칸이라면,
        {
            inventory.items[droppedItem.item_slotID] = new ConsumableItem(); // 옮길려는 아이템이 차지하고 있던 Dictionay의 자리에 빈 값을 넣어줌
            inventory.items[slot_slotID] = droppedItem.item; // 빈 칸인 Dictionay에 이동시키려는 아이템의 정보를 넣어줌
            droppedItem.item_slotID = slot_slotID; // 이동시키려는 아이템의 슬롯 위치의 id값을 이사갈 자리의 id값으로 바꿈
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

        }
        else if (droppedItem.item_slotID != slot_slotID)
        {
            Transform item = this.transform.GetChild(0); // droppedItem이 이사갈 자리에 이미 있는 아이템

            inventory.items[droppedItem.item_slotID] = item.GetComponent<ItemData>().item;
            inventory.items[slot_slotID] = droppedItem.item;
            
            item.GetComponent<ItemData>().item_slotID = droppedItem.item_slotID;
            item.transform.SetParent(inventory.slots[droppedItem.item_slotID].transform);
            item.transform.position = inventory.slots[droppedItem.item_slotID].transform.position;

            droppedItem.item_slotID = slot_slotID;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI Slot �����տ� �ٴ� ��ũ��Ʈ
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
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>(); // �̵���Ű���� ������
        if (inventory.items[slot_slotID].index == -1) // droppedItem�� �̻簥 �ڸ��� �̹� �ִ� �������� �� ĭ�̶��,
        {
            inventory.items[droppedItem.item_slotID] = new ConsumableItem(); // �ű���� �������� �����ϰ� �ִ� Dictionay�� �ڸ��� �� ���� �־���
            inventory.items[slot_slotID] = droppedItem.item; // �� ĭ�� Dictionay�� �̵���Ű���� �������� ������ �־���
            droppedItem.item_slotID = slot_slotID; // �̵���Ű���� �������� ���� ��ġ�� id���� �̻簥 �ڸ��� id������ �ٲ�
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

        }
        else if (droppedItem.item_slotID != slot_slotID)
        {
            Transform item = this.transform.GetChild(0); // droppedItem�� �̻簥 �ڸ��� �̹� �ִ� ������

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

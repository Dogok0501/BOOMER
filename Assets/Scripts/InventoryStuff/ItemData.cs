using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI Item 프리팹에 붙는 스크립트
public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ConsumableItem item; // Inventory class line.81
    public int amount;
    public int item_slotID; // 아이템이 있는 slot의 위치, Inventory class line.82

    Inventory inventory;
    Tooltip tooltip;
    PlayerHealth playerHealth;
    Weapon weapon;
    Vector2 offset;

    void Start()
    {
        inventory = GameObject.Find("InventoryThings").GetComponent<Inventory>();
        tooltip = inventory.GetComponent<Tooltip>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        weapon = GameObject.Find("Player").GetComponent<Weapon>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {           
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false; //ray가 현재 드래그 중인 아이템 ui를 뚫음
        }
    }    

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventory.slots[item_slotID].transform);
        this.transform.position = inventory.slots[item_slotID].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            if (item != null)
            {
                Debug.Log(item.index);
                // 아이템 효과발동

                switch (item.index)
                {
                    case 1000:
                        playerHealth.AddHealth(item.value);
                        break;
                    case 1001:
                        playerHealth.AddArmor(item.value);
                        break;
                    case 1002:
                        weapon.ObtainAmmo(item.value);
                        break;
                    default:
                        break;
                }

                inventory.SubItem(item.index);
                tooltip.Deactivate();
            }
        }
    }
}

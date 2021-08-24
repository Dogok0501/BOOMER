using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interactable
{
    float openSpeed = 5f;

    bool canOpen = true;

    Inventory inventory;

    private void Start()
    {
        inventory = Util.FindChild<Inventory>(GameObject.FindGameObjectWithTag("Player"),null,true);
    }

    private void Update()
    {
        DoorOpening();
    }    

    void DoorOpening()
    {        
        if (canOpen == false && this.transform.localPosition.y != -6.2f && CheckEngram())
        {            
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(this.transform.localPosition.x, -6.2f, this.transform.localPosition.z), openSpeed * Time.deltaTime);
            if (this.transform.localPosition.y == -6.2f)
            {
                this.transform.gameObject.SetActive(false);
                inventory.SubItem(1003);
            }
        }        
    }

    bool CheckEngram()
    {
        int engramAmount = 0;

        for (int j = 0; j < inventory.slotAmount; j++)
        {
            if (inventory.items[j].index == 1003)
            {
                ItemData engram = inventory.slots[j].transform.GetChild(0).GetComponent<ItemData>();
                engramAmount = engram.amount;
            }
        }

        if (engramAmount > 0)
            return true;
        else
            return false;
    }

    public override string GetDescription()
    {
        if (!CheckEngram())
        {
            canOpen = true;
            return "You need Engram";
        }            
        else if (canOpen)
            return "Press E to Open";
        else
            return "";
    }

    public override void Interact()
    {
        canOpen = !canOpen;
        Managers.Sound.Play("Door");
    }
}

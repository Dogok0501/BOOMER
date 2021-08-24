using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_EndingDoor : Interactable
{
    bool canOpen = true;

    Inventory inventory;

    private void Start()
    {
        inventory = Util.FindChild<Inventory>(GameObject.FindGameObjectWithTag("Player"), null, true);
    }

    private void Update()
    {
        DoorOpening();
    }

    void DoorOpening()
    {
        if (canOpen == false && CheckExoticEngram())
        {
            Managers.Scene.LoadScene(SceneType.Scene.Ending);
            Managers.Sound.Clear();
        }
    }

    bool CheckExoticEngram()
    {
        int engramAmount = 0;

        for (int j = 0; j < inventory.slotAmount; j++)
        {
            if (inventory.items[j].index == 1004)
            {
                ItemData exoticEngram = inventory.slots[j].transform.GetChild(0).GetComponent<ItemData>();
                engramAmount = exoticEngram.amount;
            }
        }

        if (engramAmount > 0)
            return true;
        else
            return false;
    }

    public override string GetDescription()
    {
        if (!CheckExoticEngram())
        {
            canOpen = true;
            return "You need Exotic Engram";
        }
        else if (canOpen)
            return "Press E to Open";
        else
            return "";
    }

    public override void Interact()
    {
        canOpen = !canOpen;
    }
}

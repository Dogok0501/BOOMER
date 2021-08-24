using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_NPC : Interactable
{
    bool talkable = true;

    int index;

    NPCInformation npcInfo;

    public override string GetDescription()
    {
        if (talkable == true)
            return "Press E to Talk";
        else
            return "Press E to Leave";
    }

    public override void Interact()
    {
        npcInfo = gameObject.GetComponent<NPCInformation>();

        index = npcInfo.npc.index;

        if (talkable == true)
        {
            Stage1Scene._dialogue.GenerateDialogue(index); // 대화창 활성화
            talkable = !talkable;
        }
        else
        {
            Stage1Scene._dialogue.EndDialogue();
            talkable = !talkable;
        }            
    }
}

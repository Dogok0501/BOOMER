using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInformation : MonoBehaviour
{
    [HideInInspector]
    public NPC npc;

    void Start()
    {
        InfoSet();
    }

    void InfoSet()
    {
        string string_name = gameObject.name.Replace("NPC_", "");

        for (int i = 2000; i < Managers.Data.npcDict.Count + 2000; i++)
        {
            Debug.Log(i);
            if (string_name == Managers.Data.npcDict[i].name)
            {
                npc = Managers.Data.npcDict[i];
                break;
            }
        }
    }
}

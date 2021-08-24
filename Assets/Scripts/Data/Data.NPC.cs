using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region NPC

[Serializable]
public class NPC
{
    public int index;
    public string name;
    public int dialogueindex;
    public int questindex;

    public NPC()
    {
        this.index = -1;
        this.name = null;
        this.dialogueindex = -1;
        this.questindex = -1;
}
}

[Serializable]
public class NPCData : ILoader<int, NPC>
{
    public List<NPC> npc = new List<NPC>();

    public Dictionary<int, NPC> MakeDict()
    {
        Dictionary<int, NPC> dict = new Dictionary<int, NPC>();
        foreach (NPC _npc in npc)
            dict.Add(_npc.index, _npc);
        return dict;
    }
}

#endregion

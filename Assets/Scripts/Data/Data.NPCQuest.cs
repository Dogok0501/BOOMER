using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region NPCQuest

[Serializable]
public class NPCQuest
{
    public int index;
    public string name;
    public string description;
    public string type;
    public int goal;
    public int targetindex;
    public int rewarditemindex;

    public NPCQuest()
    {
        this.index = -1;
        this.name = null;
        this.description = null;
        this.type = null;
        this.goal = -1;
        this.targetindex = -1;
        this.rewarditemindex = -1;
    }
}

[Serializable]
public class NPCQuestData : ILoader<int, NPCQuest>
{
    public List<NPCQuest> npcquest = new List<NPCQuest>();

    public Dictionary<int, NPCQuest> MakeDict()
    {
        Dictionary<int, NPCQuest> dict = new Dictionary<int, NPCQuest>();
        foreach (NPCQuest _npcQuest in npcquest)
            dict.Add(_npcQuest.index, _npcQuest);
        return dict;
    }
}

#endregion

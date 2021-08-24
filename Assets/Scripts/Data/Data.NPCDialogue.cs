using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region NPCDialogue

[Serializable]
public class NPCDialogue
{
    public int index;
    public string dialogue_start;
    public string dialogue_progress;
    public string dialogue_end1;
    public string dialogue_end2;


    public NPCDialogue()
    {
        this.index = -1;
        this.dialogue_start = null;
        this.dialogue_progress = null;
        this.dialogue_end1 = null;
        this.dialogue_end2 = null;
    }
}

[Serializable]
public class NPCDialogueData : ILoader<int, NPCDialogue>
{
    public List<NPCDialogue> npcdialogue = new List<NPCDialogue>();

    public Dictionary<int, NPCDialogue> MakeDict()
    {
        Dictionary<int, NPCDialogue> dict = new Dictionary<int, NPCDialogue>();
        foreach (NPCDialogue _npcdialogue in npcdialogue)
            dict.Add(_npcdialogue.index, _npcdialogue);
        return dict;
    }
}

#endregion

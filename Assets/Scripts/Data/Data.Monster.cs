using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region NPCDialogue

[Serializable]
public class Monster
{
    public int index;
    public string name;
    public int health;
    public int dropitemindex1;


    public Monster()
    {
        this.index = -1;
        this.name = null;
        this.health = -1;
        this.dropitemindex1 = -1;
    }
}

[Serializable]
public class MonsterData : ILoader<int, Monster>
{
    public List<Monster> monster = new List<Monster>();

    public Dictionary<int, Monster> MakeDict()
    {
        Dictionary<int, Monster> dict = new Dictionary<int, Monster>();
        foreach (Monster _monster in monster)
            dict.Add(_monster.index, _monster);
        return dict;
    }
}

#endregion

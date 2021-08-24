using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0. ILoader를 상속받는 클래스는 MakeDic 필수, List를 받아 Dictionary로 저장하는 메소드
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // 1. 정수값의 Key와 Consumable 형식의 데이터를 Value로 가지는 Dictionary의 정의.
    public Dictionary<int, ConsumableItem> consumableItemDict { get; private set; } = new Dictionary<int, ConsumableItem>();
    public Dictionary<int, NPC> npcDict { get; private set; } = new Dictionary<int, NPC>();
    public Dictionary<int, NPCDialogue> npcDialogueDict { get; private set; } = new Dictionary<int, NPCDialogue>();
    public Dictionary<int, NPCQuest> npcQuestDict { get; private set; } = new Dictionary<int, NPCQuest>();
    public Dictionary<int, Monster> monstertDict { get; private set; } = new Dictionary<int, Monster>();

    // 2. Manager 클래스에서 게임이 시작할 때 실행됨.
    public void Init()
    {
        consumableItemDict = LoadJson<ConsumableItemData, int, ConsumableItem>("ConsumableItemData").MakeDict(); // 7. 반환된 오브젝트의 정보를 MakeDict로 
                                                                                                                 //    껍데기 ConsumableItemData에 Key와 Value값을 집어 넣어줌.
                                                                                                                 //    json 파일의 DIctionary화 완성!
        npcDict = LoadJson<NPCData, int, NPC>("NPCData").MakeDict();
        npcDialogueDict = LoadJson<NPCDialogueData, int, NPCDialogue>("NPCDialogueData").MakeDict();
        npcQuestDict = LoadJson<NPCQuestData, int, NPCQuest>("QuestData").MakeDict();
        monstertDict = LoadJson<MonsterData, int, Monster>("MonsterData").MakeDict();
    }

    // 이 부분은 위의 두 부분과는 다르게 추가되거나 변경될 일이 없음.
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}"); // 5. json 파일의 주소를 받아서 불러와 TextAsset으로 저장하고,
        return JsonUtility.FromJson<Loader>(textAsset.text);             // 6. 읽어온 TextAsset을 오브젝트로 반환.
    }
}

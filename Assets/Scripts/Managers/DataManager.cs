using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0. ILoader�� ��ӹ޴� Ŭ������ MakeDic �ʼ�, List�� �޾� Dictionary�� �����ϴ� �޼ҵ�
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // 1. �������� Key�� Consumable ������ �����͸� Value�� ������ Dictionary�� ����.
    public Dictionary<int, ConsumableItem> consumableItemDict { get; private set; } = new Dictionary<int, ConsumableItem>();
    public Dictionary<int, NPC> npcDict { get; private set; } = new Dictionary<int, NPC>();
    public Dictionary<int, NPCDialogue> npcDialogueDict { get; private set; } = new Dictionary<int, NPCDialogue>();
    public Dictionary<int, NPCQuest> npcQuestDict { get; private set; } = new Dictionary<int, NPCQuest>();
    public Dictionary<int, Monster> monstertDict { get; private set; } = new Dictionary<int, Monster>();

    // 2. Manager Ŭ�������� ������ ������ �� �����.
    public void Init()
    {
        consumableItemDict = LoadJson<ConsumableItemData, int, ConsumableItem>("ConsumableItemData").MakeDict(); // 7. ��ȯ�� ������Ʈ�� ������ MakeDict�� 
                                                                                                                 //    ������ ConsumableItemData�� Key�� Value���� ���� �־���.
                                                                                                                 //    json ������ DIctionaryȭ �ϼ�!
        npcDict = LoadJson<NPCData, int, NPC>("NPCData").MakeDict();
        npcDialogueDict = LoadJson<NPCDialogueData, int, NPCDialogue>("NPCDialogueData").MakeDict();
        npcQuestDict = LoadJson<NPCQuestData, int, NPCQuest>("QuestData").MakeDict();
        monstertDict = LoadJson<MonsterData, int, Monster>("MonsterData").MakeDict();
    }

    // �� �κ��� ���� �� �κа��� �ٸ��� �߰��ǰų� ����� ���� ����.
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}"); // 5. json ������ �ּҸ� �޾Ƽ� �ҷ��� TextAsset���� �����ϰ�,
        return JsonUtility.FromJson<Loader>(textAsset.text);             // 6. �о�� TextAsset�� ������Ʈ�� ��ȯ.
    }
}

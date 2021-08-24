using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Item

[Serializable] // 3. 클래스를 json 문자열로 변환
public class ConsumableItem
{
    public int index;
    public string name;
    public string type;
    public string description;
    public int value;
    public bool stackable;
    public string sprite;

    public ConsumableItem() // 깡통 id를 부여함으로 빈 아이템을 의미
    {
        this.index = -1;
        this.name = null;
        this.type = null;
        this.description = null;
        this.value = -1;
        this.stackable = true;
        this.sprite = null;

    }
}

[Serializable] // 4. 클래스를 json 문자열로 변환
public class ConsumableItemData : ILoader<int, ConsumableItem>                        // interface ILoader를 상속 받았으므로 MakeDict 필수 구현
{
    public List<ConsumableItem> consumableItem = new List<ConsumableItem>();          // ConsumableItem 형식을 List로 가지는 List 정의.

    public Dictionary<int, ConsumableItem> MakeDict()                                 // 위의 받아온 List를 
    {
        Dictionary<int, ConsumableItem> dict = new Dictionary<int, ConsumableItem>(); // 임시 Dictionary에
        foreach (ConsumableItem _consumableItem in consumableItem)                    // foreach로 순회하면서
            dict.Add(_consumableItem.index, _consumableItem);                         // id 값을 key값으로 하여 저장.
        return dict;                                                                  // 아직 아무 정보도 없는 껍데기 Dictionary 반환.
    }
}

#endregion
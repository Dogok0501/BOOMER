using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Item

[Serializable] // 3. Ŭ������ json ���ڿ��� ��ȯ
public class ConsumableItem
{
    public int index;
    public string name;
    public string type;
    public string description;
    public int value;
    public bool stackable;
    public string sprite;

    public ConsumableItem() // ���� id�� �ο������� �� �������� �ǹ�
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

[Serializable] // 4. Ŭ������ json ���ڿ��� ��ȯ
public class ConsumableItemData : ILoader<int, ConsumableItem>                        // interface ILoader�� ��� �޾����Ƿ� MakeDict �ʼ� ����
{
    public List<ConsumableItem> consumableItem = new List<ConsumableItem>();          // ConsumableItem ������ List�� ������ List ����.

    public Dictionary<int, ConsumableItem> MakeDict()                                 // ���� �޾ƿ� List�� 
    {
        Dictionary<int, ConsumableItem> dict = new Dictionary<int, ConsumableItem>(); // �ӽ� Dictionary��
        foreach (ConsumableItem _consumableItem in consumableItem)                    // foreach�� ��ȸ�ϸ鼭
            dict.Add(_consumableItem.index, _consumableItem);                         // id ���� key������ �Ͽ� ����.
        return dict;                                                                  // ���� �ƹ� ������ ���� ������ Dictionary ��ȯ.
    }
}

#endregion
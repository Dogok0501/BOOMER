using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStageScene : BaseScene
{
    public static DialogueAndQuestManager _dialogue = new DialogueAndQuestManager();


    protected override void Init()
    {
        _dialogue.Init();
    }

    public override void Clear()
    {
        
    }
}

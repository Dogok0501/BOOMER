using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScene : BaseScene
{
    float waitForSecond = 2f;
    float disableTime = 5f;

    protected override void Init()
    {
        base.Init();

        sceneType = SceneType.Scene.Ending;

        BGMplay();

        StartCoroutine(Util.FadeOut<Image>("BlackFade", waitForSecond));
    }

    private void Update()
    {
        FadeToLevel();
    }

    public void FadeToLevel()
    {
        disableTime -= Time.deltaTime;
        if (disableTime > 0)
        {
            Managers.IsInputEnable = false;
        }
        else
        {
            if (Input.anyKeyDown)
            {
                Managers.IsInputEnable = true;
                StartCoroutine(Util.FadeIn<Image>("BlackFade", waitForSecond));
                StartCoroutine(Util.FadeOut<AudioSource>("BGM", waitForSecond));
                Invoke("ToNextScene", 5.0f);
                disableTime = float.MaxValue;
            }
        }
    }

    public void ToNextScene()
    {
        Managers.Scene.LoadScene(SceneType.Scene.Title);
        Clear();
    }

    void BGMplay()
    {
        Managers.Sound.Play("BGMs/Victory", SoundManager.SoundType.BGM);
    }

    public override void Clear()
    {
        Managers.Sound.Clear();
    }
}

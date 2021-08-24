using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    float waitForSecond = 5f;
    float disableTime = 7f;

    protected override void Init()
    {
        base.Init();

        sceneType = SceneType.Scene.Title;

        BGMplay();
        StartCoroutine(Util.FadeIn<AudioSource>("BGM", waitForSecond));
        StartCoroutine(Util.FadeOut<Image>("BlackFade", waitForSecond));
        StartCoroutine(Util.FadeIn<Image>("TitleTitle", waitForSecond / Time.deltaTime * 0.016f));
        StartCoroutine(Util.FadeIn<Text>("StartText", waitForSecond / Time.deltaTime * 0.08f));
    }

    private void Update()
    {
        FadeToLevel();
    }

    void BGMplay()
    {
        Managers.Sound.Play("BGMs/Title", SoundManager.SoundType.BGM);
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
                StartCoroutine(Util.FadeOut<AudioSource>("BGM", waitForSecond));
                StartCoroutine(Util.FadeIn<Image>("BlackFade", waitForSecond));
                Invoke("ToNextScene", 5.0f);
                disableTime = float.MaxValue;
            }
        }
        
    }

    public void ToNextScene()
    {
        LoadingSceneManager.LoadScene(SceneType.Scene.MainMenu);
        Clear();
    }

    public override void Clear()
    {
        Managers.Sound.Clear();
    }
}

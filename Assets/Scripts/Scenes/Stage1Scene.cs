using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage1Scene : BaseStageScene
{
    GameObject menuPanel;
    Button quitButton;

    protected override void Init()
    {
        base.Init();

        sceneType = SceneType.Scene.Stage1;

        BGMplay();

        menuPanel = GameObject.Find("Menu Panel");
        menuPanel.SetActive(false);

        quitButton = menuPanel.transform.Find("Quit Button").GetComponent<Button>();

        StartCoroutine(Util.FadeOut<Image>("BlackFadeStart", 1f));
    }

    void BGMplay()
    {
        Managers.Sound.Play("BGMs/AtDoomsGate", SoundManager.SoundType.BGM);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuPanel.activeSelf && !Managers.gameIsPaused)
            {
                Managers.Scene.Pause();
                menuPanel.SetActive(true);
            }
            else if (menuPanel.activeSelf && Managers.gameIsPaused)
            {
                Managers.Scene.Resume();
                menuPanel.SetActive(false);
            }
        }

        quitButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public override void Clear()
    {
        Managers.Sound.Clear();
    }    
}

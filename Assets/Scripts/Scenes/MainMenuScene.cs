using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : BaseScene
{
    public Button playButton;
    public Button optionButton;
    public Button quitButton;
    public Slider volumeSlider;

    protected override void Init()
    {
        BGMplay();

        base.Init();

        sceneType = SceneType.Scene.MainMenu;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        playButton.onClick.AddListener(ToNextScene);
        quitButton.onClick.AddListener(QuitGame);
    }

    void BGMplay()
    {
        Managers.Sound.Play("BGMs/AtDoomsGate", SoundManager.SoundType.BGM);
    }

    public void ToNextScene()
    {
        LoadingSceneManager.LoadScene(SceneType.Scene.Stage1);
        Clear();
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

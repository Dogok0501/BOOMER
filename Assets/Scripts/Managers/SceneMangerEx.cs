using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }


    string GetSceneName(SceneType.Scene type)
    {
        string name = System.Enum.GetName(typeof(SceneType.Scene), type);
        return name;
    }

    public void LoadScene(SceneType.Scene type)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }

    public void Pause()
    {        
        GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerLook>().enabled = false;
        GameObject.Find("Player").GetComponent<Weapon>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        Managers.gameIsPaused = true;
    }

    public void Resume()
    {
        GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerLook>().enabled = true;
        GameObject.Find("Player").GetComponent<Weapon>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        Managers.gameIsPaused = false;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
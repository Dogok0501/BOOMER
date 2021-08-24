using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    Image progressBar;
    public static SceneType.Scene sceneType;

    private void Start()
    {
        progressBar = GameObject.Find("Progress Bar").GetComponent<Image>();
        StartCoroutine(LoadAsynchronously());
    }

    string GetSceneName(SceneType.Scene type)
    {
        string name = System.Enum.GetName(typeof(SceneType.Scene), type);
        return name;
    }

    public static void LoadScene(SceneType.Scene type)
    {
        Managers.Clear();
        sceneType = type;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadAsynchronously()
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(GetSceneName(sceneType));
        operation.allowSceneActivation = false;

        float timer = 0.0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (operation.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, operation.progress, timer);
                if(progressBar.fillAmount >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if(progressBar.fillAmount == 1.0f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

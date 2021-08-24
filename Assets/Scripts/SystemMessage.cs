using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessage : MonoBehaviour
{
    public Text messageText;
    public static SystemMessage instance;
    float duration = 1f;
    float smoothness = 0.02f;

    private void Awake()
    {
        messageText = this.GetComponent<Text>();
        instance = this;
    }

    void Start()
    {        
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0);        
    }    

    public IEnumerator TextUpdate(string text)
    {
        messageText.text = text;
        float progress = 0;
        float increment = smoothness / duration;

        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1);

        while (progress < 1)
        {
            Debug.Log("asdasdasaa");
            messageText.color = Color.Lerp(messageText.color, new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0), progress);
            progress += increment;
            yield return new WaitForSecondsRealtime(smoothness);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Util
{
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        if(recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
    

    public static IEnumerator FadeOut<T>(string name, float fadeTime)
    {
        GameObject _obj = GameObject.Find(name);

        if (_obj == null)
        {
            Debug.Log($"Can't Find Object name {name}");
            yield break;
        }

        T obj = _obj.GetComponent<T>();

        if( typeof(T) == typeof(AudioSource) )
        {
            AudioSource nowPlaying = obj as AudioSource;
            float startVolume = nowPlaying.volume;

            while (nowPlaying.volume > 0)
            {
                nowPlaying.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            nowPlaying.Stop();
            nowPlaying.volume = startVolume;
        }

        if (typeof(T) == typeof(Image))
        {
            Image img = obj as Image;
            img.enabled = true;
            float startAlpha = img.color.a;

            while (img.color.a > 0)
            {
                startAlpha -= Time.deltaTime / fadeTime;
                img.color = new Color(img.color.r, img.color.g, img.color.b, startAlpha);

                yield return null;
            }

            img.enabled = false;
            img.enabled = true;

            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }

        if (typeof(T) == typeof(Text))
        {
            Text text = obj as Text;
            text.enabled = true;
            float startAlpha = text.color.a;

            while (text.color.a > 0)
            {
                startAlpha -= Time.deltaTime / fadeTime;
                text.color = new Color(text.color.r, text.color.g, text.color.b, startAlpha);

                yield return null;
            }

            text.enabled = false;
            text.enabled = true;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }
    }

    public static IEnumerator FadeIn<T>(string name, float fadeTime)
    {
        GameObject _obj = GameObject.Find(name);
        if (_obj == null)
        {
            Debug.Log($"Can't Find Object name {name}");
            yield break;
        }

        T obj = _obj.GetComponent<T>();

        if (typeof(T) == typeof(AudioSource))
        {
            AudioSource nowPlaying = obj as AudioSource;
            float startVolume = nowPlaying.volume;
            nowPlaying.volume = 0;

            while (nowPlaying.volume < 1)
            {
                nowPlaying.volume += startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            nowPlaying.volume = startVolume;
        }

        if (typeof(T) == typeof(Image))
        {
            Image img = obj as Image;
            img.enabled = true;
            float startAlpha = img.color.a;

            while (img.color.a < 1.0f)
            {
                startAlpha += Time.deltaTime / fadeTime;
                img.color = new Color(img.color.r, img.color.g, img.color.b, startAlpha);

                yield return null;
            }

            img.enabled = false;
            img.enabled = true;

            img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
        }

        if (typeof(T) == typeof(Text))
        {
            Text text = obj as Text;
            text.enabled = true;
            float startAlpha = text.color.a;

            while (text.color.a < 1.0f)
            {
                startAlpha += Time.deltaTime / fadeTime;
                text.color = new Color(text.color.r, text.color.g, text.color.b, startAlpha);

                yield return null;
            }

            text.enabled = false;
            text.enabled = true;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
        }

        if (typeof(T) == typeof(CanvasGroup))
        {
            CanvasGroup canvas = obj as CanvasGroup;
            canvas.enabled = true;
            float startAlpha = canvas.alpha;

            while (canvas.alpha < 1.0f)
            {
                startAlpha += Time.deltaTime / fadeTime;

                yield return null;
            }

            canvas.enabled = false;
            canvas.enabled = true;

            canvas.alpha = 1.0f;
        }
    }
}

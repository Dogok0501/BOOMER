using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    public enum SoundType { BGM, Effect, MaxCount }

    AudioSource[] _audioSources = new AudioSource[(int)SoundType.MaxCount];

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("Sound");
        if (root == null)
        {
            root = new GameObject { name = "Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(SoundType));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)SoundType.BGM].loop = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, SoundType type = SoundType.Effect, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        if (type == SoundType.BGM)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            if(audioClip == null)
            {
                Debug.Log($"BGM AudioClip Missing! {path}");
                return;
            }

            AudioSource audioSource = _audioSources[(int)SoundType.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioClip audioClip = GetOrAddAudioClip(path);
            if (audioClip == null)
            {
                Debug.Log($"Effect AudioClip Missing! {path}");
                return;
            }

            AudioSource audioSource = _audioSources[(int)SoundType.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);

        }
    }    

    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (_audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Resources.Load<AudioClip>(path);
            _audioClips.Add(path, audioClip);            
        }
        return audioClip;
    }
}

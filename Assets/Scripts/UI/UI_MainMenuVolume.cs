using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_MainMenuVolume : MonoBehaviour
{
    public Slider BGMSlider;
    public AudioSource BGM;

    public Slider effectSlider;
    public AudioSource effect;

    void Start()
    {
        if(this.gameObject.name == "BGMSlider")
        {
            BGMSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
            BGM = GameObject.Find("BGM").GetComponent<AudioSource>();

            BGMSlider.value = BGM.volume;
        }
        else
        {
            effectSlider = GameObject.Find("EffectSlider").GetComponent<Slider>();
            effect = GameObject.Find("Effect").GetComponent<AudioSource>();

            effectSlider.value = effect.volume;
        }
        
    }

    void Update()
    {
        if (this.gameObject.name == "BGMSlider")
        {
            BGM.volume = BGMSlider.value;
        }
        else
        {
            effect.volume = effectSlider.value;
        }
    }
}

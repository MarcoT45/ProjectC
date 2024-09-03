using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour {

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;

    private void Start() {
        if(PlayerPrefs.GetFloat("MasterVolume") == 0) {
            masterSlider.value = 1.0f;
        } else {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }

        if(PlayerPrefs.GetFloat("MusicVolume") == 0) {
            musicSlider.value = 1.0f;
        } else {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        } 

        if(PlayerPrefs.GetFloat("SoundFXVolume") == 0) {
            soundFXSlider.value = 1.0f;
        } else {
            soundFXSlider.value = PlayerPrefs.GetFloat("SoundFXVolume");
        } 
    }

}
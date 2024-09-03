using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour {

    private static SoundMixerManager instance = null;
    public static SoundMixerManager Instance => instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // En haut le singleton 
    // En bas la gestion sonore

    [SerializeField] private AudioMixer audioMixer;

    private void Start() {
        if(PlayerPrefs.GetFloat("MasterVolume") == 0) PlayerPrefs.SetFloat("MasterVolume", 1.0f);
        if(PlayerPrefs.GetFloat("MusicVolume") == 0) PlayerPrefs.SetFloat("MusicVolume", 1.0f);
        if(PlayerPrefs.GetFloat("SoundFXVolume") == 0) PlayerPrefs.SetFloat("SoundFXVolume", 1.0f);

        audioMixer.SetFloat("masterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20f);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SoundFXVolume")) * 20f);
    }

    public void SetMasterVolume(float level) {
        PlayerPrefs.SetFloat("MasterVolume", level);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level) {
        PlayerPrefs.SetFloat("MusicVolume", level);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level) {
        PlayerPrefs.SetFloat("SoundFXVolume", level);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }
    
}
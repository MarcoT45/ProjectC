using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    
    private static MusicManager instance = null;
    public static MusicManager Instance => instance;

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

    [SerializeField] private AudioSource musicAudioSource;

    public void PlayMusicClip(AudioClip audioClip) {
        musicAudioSource.clip = audioClip;
        musicAudioSource.Play();
    }

}
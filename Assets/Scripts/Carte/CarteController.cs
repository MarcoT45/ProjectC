using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarteController : MonoBehaviour {

    [SerializeField] private AudioClip titleScreenTrack;

    private void Start() {
        MusicManager.Instance.PlayMusicClip(titleScreenTrack);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour {

    [SerializeField] private AudioClip titleScreenTrack;

    private void Start() {
        MusicManager.Instance.PlayMusicClip(titleScreenTrack);
    }

}
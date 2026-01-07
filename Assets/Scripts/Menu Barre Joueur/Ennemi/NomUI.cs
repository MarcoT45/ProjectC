using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;
using DG.Tweening;

public class NomUI : MonoBehaviour
{
    [Header("References")]
    public TMP_Text text;
    public Image strip;

    [Header("Options")]
    public float timerBeforeHiding;
    public float fadeInSpeed;
    public float fadeOutSpeed;

    private float timer;

    void Awake()
    {
        //Initialement invisible
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        strip.color = new Color(strip.color.r, strip.color.g, strip.color.b, 0f);
    }

    void Start()
    {
        text.DOFade(1f, fadeInSpeed);
        strip.DOFade(1f, fadeInSpeed);

        timer = timerBeforeHiding;
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            text.DOFade(0f, fadeOutSpeed);
            //Callback ici une fois l'animation terminée
            strip.DOFade(0f, fadeOutSpeed).OnComplete(() => this.gameObject.SetActive(false));

        }
    }
}

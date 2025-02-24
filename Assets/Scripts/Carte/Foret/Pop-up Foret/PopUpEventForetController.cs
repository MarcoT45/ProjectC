using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpEventForetController : MonoBehaviour {

    // Elements de la pop-up
    public GameObject conteneur;
    public GameObject buissonGauche;
    public GameObject buissonDroite;
    public GameObject partieCentrale;
    public GameObject zoneTexteBas;

    // Sons de la pop-up
    public AudioClip bushSFXTrack;

    public void GeneratePopUpEvent(Noeud n) {
        // Mettre l'image au centre de l'herbe (ici) (on peut utiliser ca dans une fonction init event qui prépare l'image et le texte)
        OpenPopUpAnimation();
    }

    private async void OpenPopUpAnimation() {
        this.gameObject.SetActive(true);
        await conteneur.transform.DOScale(new Vector3(1, 1 ,1), 2f).AsyncWaitForCompletion();

        partieCentrale.GetComponent<Image>().DOFade(1.0f, 2.5f);
        await partieCentrale.GetComponent<RectTransform>().DOAnchorPosY(10, 2.5f, false).AsyncWaitForCompletion();

        buissonGauche.GetComponent<RectTransform>().DOAnchorPosX(-200, 2.5f, false);
        buissonDroite.GetComponent<RectTransform>().DOAnchorPosX(200, 2.5f, false);
        SoundFXManager.Instance.PlaySoundFXClip(bushSFXTrack, this.transform);

        zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-55, 2.0f, false);
    }

}
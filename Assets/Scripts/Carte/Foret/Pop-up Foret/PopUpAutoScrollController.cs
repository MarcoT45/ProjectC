using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PopUpAutoScrollController : MonoBehaviour {

    public GameObject textContainer;
    public GameObject textObject;
    public TextMeshProUGUI textValue;
    public float marginScroll;

    private string lastString = "";
    private bool loopEnded = true;
    private float positionYScroll = 0;

    private void FixedUpdate() {
        if(lastString != textValue.text) {
            DOTween.KillAll(textObject.transform);
            textObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            lastString = textValue.text;
            loopEnded = true;
            positionYScroll = 0;

            RectTransform c = textContainer.transform.GetComponent<RectTransform>();
            RectTransform o = textObject.transform.GetComponent<RectTransform>();
            float cHeight = c.sizeDelta.y * c.localScale.y;
            float oHeight = o.sizeDelta.y * o.localScale.y;

            if(oHeight > (cHeight + marginScroll)) {
                positionYScroll = (oHeight-cHeight);
                loopEnded = false;
                StartLoopingScroll();
            }
        } else {
            if(loopEnded) {
                loopEnded = false;
                textObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                StartLoopingScroll();
            }
        }
    }

    private void StartLoopingScroll() {
        textObject.GetComponent<RectTransform>().DOAnchorPosY(0, 1.0f, false).onComplete = ScrollingDesc;
    }

    private void ScrollingDesc() {
        textObject.GetComponent<RectTransform>().DOAnchorPosY(positionYScroll, 3.5f, false).onComplete = ScrollingEnd;
    }

    private void ScrollingEnd() {
        textObject.GetComponent<RectTransform>().DOAnchorPosY(positionYScroll, 1.0f, false).onComplete = EndLoop;
    }

    private void EndLoop() {
        loopEnded = true;
    }

}
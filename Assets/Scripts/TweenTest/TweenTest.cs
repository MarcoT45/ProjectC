using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ennemy.transform.DOMoveY(0.2f,1f, true)
        /*transform.DOLocalJump(transform.position, 1f, 1, 0.5f)
             .SetLoops(5, LoopType.Yoyo)
             .SetEase(Ease.InOutQuint);*/

        Camera cam = Camera.main;

        cam.DOShakePosition(1f, 1f, 5, 90, true, ShakeRandomnessMode.Harmonic)
             .SetLoops(5, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

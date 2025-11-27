using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Les types de collectibles possibles
public enum CollectibleType {
    Coin,
    Loot
    //Health,
    //Key
}

public class Collectible : MonoBehaviour {
    
    public CollectibleType collectibleType;
    public int amount = 1;

    public void Start() {
        /*
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBack);
        */

        transform.DOMoveY(transform.position.y + 0.2f, 0.6f)
             .SetLoops(-1, LoopType.Yoyo)
             .SetEase(Ease.InOutSine);

        /* transform.DORotate(new Vector3(0, 0, 360), 1.2f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
        */
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.Collect(this);
            }
            DOTween.Kill(this.transform);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Enemy")) {
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null) {
                enemyAI.Collect(this);
            }
            DOTween.Kill(this.transform);
            Destroy(gameObject);
        }
    }

}
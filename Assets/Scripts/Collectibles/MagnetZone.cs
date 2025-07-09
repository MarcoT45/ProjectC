using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetZone : MonoBehaviour
{
    public float attractionSpeed = 5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            collision.transform.position += direction * attractionSpeed * Time.deltaTime;
        }
    }
}

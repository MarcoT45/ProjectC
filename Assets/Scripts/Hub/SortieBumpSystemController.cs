using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SCRIPT A SUPPRIMER QUAND IL NE SERA PLUS NECESSAIRE D'ACCEDER A BUMPSYSTEM

public class SortieBumpSystemController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            SceneManager.LoadScene(4);
        }
    }
}

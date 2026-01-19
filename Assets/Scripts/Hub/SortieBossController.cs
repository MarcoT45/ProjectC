using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// SCRIPT A SUPPRIMER QUAND IL NE SERA PLUS NECESSAIRE D'ACCEDER A BUMPSYSTEM

public class SortieBossController : MonoBehaviour
{
    public int sceneIndex = 0;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}

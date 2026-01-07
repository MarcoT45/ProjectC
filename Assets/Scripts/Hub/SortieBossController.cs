using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SCRIPT A SUPPRIMER QUAND IL NE SERA PLUS NECESSAIRE D'ACCEDER A BUMPSYSTEM

public class SortieBossController : MonoBehaviour
{
    public bool avecCutscene = false;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if(avecCutscene)
            {
                SceneManager.LoadScene(6);
            }
            else
            {
                SceneManager.LoadScene(5);
            }
        }
    }
}

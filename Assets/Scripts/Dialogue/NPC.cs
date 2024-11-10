using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

//Classe abstraite pour les NPCs
public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer interactSprite;
    private bool isColliding;
    protected GameObject collidingPlayer;


    protected virtual void Update()
    {
        //Si controls du Perso
        if(ControlsManager.Instance.controlsState == ControlsState.CharacterHub)
        {
            if (ControlsManager.Instance.ValiderPressed && isColliding)
            {
                ControlsManager.Instance.UpdateState(6);
                Interact();
            }
        }
        //Si controls du dialogue
        else if (ControlsManager.Instance.controlsState == ControlsState.Dialogue)
        {
             if (ControlsManager.Instance.ValiderPressed && isColliding)
             {
                 Interact();
             }
        }

        //Gère l'apparition de l'icone d'intéraction 
        if (interactSprite.gameObject.activeSelf && !isColliding)
        {
            interactSprite.gameObject.SetActive(false);

        }
        else if (!interactSprite.gameObject.activeSelf && isColliding)
        {
            interactSprite.gameObject.SetActive(true);
        }
    }

    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = true;
            collidingPlayer = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = false;
            collidingPlayer = null;
        }
    }
}

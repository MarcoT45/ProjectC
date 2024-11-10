using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueText dialogueText;
    private bool isColliding;

    private void Update()
    {
        if (ControlsManager.Instance.ValiderPressed && isColliding)
        {
            TriggerDialogue(dialogueText);
        }
    }

    public void TriggerDialogue(DialogueText dialogueText)
    {
        DialogueManager.Instance.DisplayDialogue(dialogueText);
    }
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isColliding = false;
        }
    }
}

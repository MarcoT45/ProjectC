using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    private static DialogueManager instance = null;
    public static DialogueManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueBoxText;
    [SerializeField] private ChoiceBox choiceBox;

    private Queue<string> dialogueQueue = new Queue<string>();
    private string p;
    private bool isTyping;
    private Coroutine typingDialogueCoroutine;
    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_TIME = 0.1f;

    public bool dialogueEnded;
    public float typingSpeed = 10f;

    public delegate void OnStartDialog();
    public static event OnStartDialog onStartDialog;

    public delegate void OnEndDialog();
    public static event OnEndDialog onEndDialog;

    public void Start()
    {
        // On désactive tout
        dialogueEnded = false;
        isTyping = false;
        dialogueBox.SetActive(false);
    }
    
    public void DisplayDialogue(DialogueText dialogueText, Action<int> onChoiceSelected = null)
    {

        //Si il n'y a rien dans la queue
        if(dialogueQueue.Count == 0)
        {
            //On commence le dialogue
            if(!dialogueEnded)
            {
                StartDialogue(dialogueText);
            }

            //Fin du dialogue avec choix
            else if (dialogueEnded && !isTyping && (dialogueText.optionChoices != null && dialogueText.optionChoices.Count > 1))
            {
                ControlsManager.Instance.UpdateState(7);
                choiceBox.ShowChoices(dialogueText.optionChoices, onChoiceSelected);

                return;
            }
            //Fin du dialogue sans choix
            else if (dialogueEnded && !isTyping )
            {
                EndDialogue();
                ControlsManager.Instance.UpdateState(5);
                return;
            }
        }

        //Si il y a quelque chose dans la queue
        if (!isTyping)
        {
            p = dialogueQueue.Dequeue();
            typingDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }
        //On interrompt le typing du dialogue
        else
        {
            FinishParagraphEarly();
        }

        //On précise que le dialogue est en cours
        if (dialogueQueue.Count == 0)
        {
            dialogueEnded = true ;
        }
    }


    private void StartDialogue(DialogueText dialogueText)
    {
        //Active GO
        if(!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
        }

        //Update le nom
        nameText.text = dialogueText.speakerName + " :";

        //Ajoute les dialogues à la queue
        for (int i = 0; i < dialogueText.paragraphs.Length; i++)
        {
            dialogueQueue.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    public void EndDialogue()
    {
        //clear queue
        dialogueQueue.Clear();

        dialogueEnded = false;

        if(dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
        }

    }


    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        dialogueBoxText.text = p;
        dialogueBoxText.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in p.ToCharArray())
        {

            maxVisibleChars++;
            dialogueBoxText.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typingSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        //stop la coroutine
        StopCoroutine(typingDialogueCoroutine);

        //afficher le texte
        dialogueBoxText.text = p;

        isTyping = false;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] GameObject choiceTextPrefab;

    private List<ChoiceText> choiceTextList = new List<ChoiceText>();
    private int currentChoice;
    private Action<int> choiceAction;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowChoices(List<string> choices, Action<int> onChoiceSelected)
    {
        GameObject choiceTextObj;

        gameObject.SetActive(true);  
        currentChoice = 0;
        
        //Détruire les choix existant
        choiceTextList.Clear();
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //Ajouter les nouveaux
        foreach(string choice in choices)
        {
            choiceTextObj = Instantiate(choiceTextPrefab, transform);
            choiceTextObj.GetComponentInChildren<TMP_Text>().text = choice;
            choiceTextObj.transform.Find("Icone").gameObject.SetActive(false);
            choiceTextList.Add(choiceTextObj.GetComponent<ChoiceText>());
        }

        //Récupèrer l'action envoyé en paramètre
        choiceAction = onChoiceSelected;

    }

    //Fonction lançant l'action voulu lorque le choix est fait
    private void OnChoiceSelected()
    {
        choiceAction?.Invoke(currentChoice);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ControlsManager.Instance.controlsState == ControlsState.DialogueChoice)
        {
            //Change le choix suivant l'input 
            if (ControlsManager.Instance.DeplacerPressed && ControlsManager.Instance.DeplacerValue == Vector2.up)
            {
                currentChoice--;
            }
            else if (ControlsManager.Instance.DeplacerPressed && ControlsManager.Instance.DeplacerValue == Vector2.down)
            {
                currentChoice++;
            }

            //Limite le choix entre 0 et taille de la liste - 1
            currentChoice = Mathf.Clamp(currentChoice, 0, choiceTextList.Count - 1);

            //Validation du choix
            if (ControlsManager.Instance.ValiderPressed)
            {
                OnChoiceSelected();
            }
        }

        //Boucle pour afficher si le choix est sélectionné
        for (int i = 0; i < choiceTextList.Count; i++)
        {
            choiceTextList[i].SetSelected(i == currentChoice);
        }

    }
}

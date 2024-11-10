using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
public class DialogueText : ScriptableObject
{
    public string speakerName;

    [TextArea(1,2)]
    public string[] paragraphs;


    public List<string> optionChoices;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceText : MonoBehaviour
{

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            this.gameObject.transform.Find("Icone").gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.transform.Find("Icone").gameObject.SetActive(false);

        }

    }
}

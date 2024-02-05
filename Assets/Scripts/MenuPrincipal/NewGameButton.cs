using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    private Button newGameButton;

    void Start()
    {
        newGameButton = this.gameObject.GetComponent<Button>();
        newGameButton.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene(1);
    }
}

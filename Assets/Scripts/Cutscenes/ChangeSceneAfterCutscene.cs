using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterCutscene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private AsyncOperation asyncLoad;   
    void Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
    }

    public void ChangeScene()
    {
        asyncLoad.allowSceneActivation = true;
    }
}

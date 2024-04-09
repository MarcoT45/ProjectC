using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeBattleButtonController : MonoBehaviour {

    public void ReturnToMap() {
        SceneManager.LoadScene(1);
    }

}
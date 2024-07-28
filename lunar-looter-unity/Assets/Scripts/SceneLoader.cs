using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Based on code by Rehope Games
public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    // **UNCOMMENT WHEN ALL LEVELS ARE IMPLEMENTED
    // Stores all level buttons
    //public Button[] buttons;

    // Keeps track of which levels are locked and which are unlocked
    /*private void Awake() {

        for(int i = 1; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 2);
        for(int i = 1; i < unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }*/

    // Loads scene that matches the name inputted into parameter
    public void Load(int levelIndex) {
        StartCoroutine(LoadSceneAsynchronously(levelIndex));
    }

    IEnumerator LoadSceneAsynchronously(int levelIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        loadingScreen.SetActive(true);
        while(!operation.isDone){
            //Debug.Log(operation.progress);
            yield return null;
        }
    }
}

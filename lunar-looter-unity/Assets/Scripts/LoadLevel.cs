using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Based on code by Rehope Games
public class LoadLevel : MonoBehaviour
{
    // Stores all level buttons
    //public Button[] buttons;

    // Keeps track of which levels are locked and which are unlocked
    /*private void Awake() {

        for(int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //Debug.Log(unlockedLevel);
        for(int i = 0; i < unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }*/

    // Loads scene that matches the name inputted into parameter
    public void Load(string levelName) {
        SceneManager.LoadScene(levelName);
    }
}

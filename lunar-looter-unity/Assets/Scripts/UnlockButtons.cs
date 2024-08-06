using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Based on code by Rehope Games
public class UnlockButtons : MonoBehaviour
{
    // Stores all level buttons
    public Button[] buttons;

    // Keeps track of which levels are locked and which are unlocked
    private void Awake() {

        for(int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i = 0; i < unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }
}

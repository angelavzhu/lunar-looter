using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReset : MonoBehaviour
{
    public Button[] buttons;

    public void Reset(){
        PlayerPrefs.DeleteAll();
        for(int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }

        //PlayerPrefs.SetInt("UnlockedLevel", 1);
        buttons[0].interactable = true;
    }
}

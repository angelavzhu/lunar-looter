using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private Boolean frozen;
    // Start is called before the first frame update
    void Start()
    {
        frozen = false;
    }

    public void Clicked()
    {
        if(frozen) {
            ResumeScreen();
        } else {
            FreezeScreen();
        }
    }

    public void FreezeScreen() {
        Time.timeScale = 0;
        AudioListener.volume = 0;
        frozen = true;
    }

    public void ResumeScreen() {
        Time.timeScale = 1;
        AudioListener.volume = 1;
        frozen = false;
    }
}

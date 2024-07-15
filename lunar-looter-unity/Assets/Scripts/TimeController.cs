using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Based on code by Turbo Makes Games
public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    public TMP_Text timeCounter;
    private TimeSpan timePlaying;
    private bool timerGoing;
    private float elapsedTime;
    
    private void Awake(){
        instance = this;
    }

    private void Start(){
        timeCounter.text = "00:00";
        timerGoing = false;
    }

    public void BeginTimer(){
        timerGoing = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer(){
        timerGoing = false;
    }

    private IEnumerator UpdateTimer(){
        while(timerGoing){
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingText = timePlaying.ToString("mm':'ss");
            timeCounter.text = timePlayingText;

            yield return null;
        }
    }
}

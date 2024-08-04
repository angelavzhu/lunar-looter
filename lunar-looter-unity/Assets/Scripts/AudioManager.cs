using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    // Gets audio source for music
    [SerializeField] private AudioSource musicSource;

    // Plays music
    public void Start(){
        musicSource.Play();
    }
}

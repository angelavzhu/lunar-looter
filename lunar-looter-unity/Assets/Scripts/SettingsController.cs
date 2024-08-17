using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static SettingsController instance;
    // Game audio source
    [SerializeField] private AudioMixer mainMixer;
    // Game master audio control
    [SerializeField] private Slider masterControl;
    // Game SFX audio control
    [SerializeField] private Slider sfxControl;
    // Game brightness control
    [SerializeField] private Slider brightnessControl;

    // Data at very beginning of game
    private float volume = 1f;
    private float sfx = 1f;
    private float brightness = 0.5f;
    private int fullScreen = 1;

    private SpriteRenderer[] spriteRenderers;

    // Sets all settings to saved data if changes were made
    public void Start(){
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        volume = PlayerPrefs.GetFloat("volume", 1f);
        sfx = PlayerPrefs.GetFloat("sfx", 1f);
        brightness = PlayerPrefs.GetFloat("brightness", 0.5f);
        fullScreen = PlayerPrefs.GetInt("fullscreen", 1);

        masterControl.value = volume;
        sfxControl.value = sfx;
        brightnessControl.value = brightness;

        if(fullScreen == 0){
            Screen.fullScreen = false;
        }
        else{
            Screen.fullScreen = true;
        }
    }

    // Sets game volume
    public void SetVolume(float inputVolume){
        volume = inputVolume;
        mainMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    // Sets SFX volume
    public void SetSfxVolume(float inputVolume){
        sfx = inputVolume;
        mainMixer.SetFloat("SFX", Mathf.Log10(sfx)*20);
        PlayerPrefs.SetFloat("SFX", sfx);
        PlayerPrefs.Save();
    }

    // Sets brightness
    public void AdjustBrightness(float BrightnessValue){
        brightness = BrightnessValue;
        for(int i = 0; i < spriteRenderers.Length; i++){
            spriteRenderers[i].color = new Color(brightness, brightness, brightness, spriteRenderers[i].color.a);
        }

        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();
    }

    // Sets game to fullscreen
    public void SetFullScreen(bool isFullScreen){
        if(isFullScreen){
            fullScreen = 1;
        }
        else{
            fullScreen = 0;
        }

        PlayerPrefs.SetInt("fullscreen", fullScreen);
        PlayerPrefs.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    // Game audio source
    [SerializeField] private AudioMixer mainMixer;
    // Game SFX control
    [SerializeField] private Slider sfxControl;

    private SpriteRenderer[] spriteRenderers;

    // Sets all settings to saved data if changes were made
    public void Start(){
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();

       /* if(PlayerPrefs.HasKey("SFX") || PlayerPrefs.HasKey("volume")){
            LoadVolume();
        }
        else{
            //SetVolume();
            SetSfxVolume();
        }*/
    }

    // Sets game volume
    public void SetVolume(float volume){
        mainMixer.SetFloat("volume", volume);

        //PlayerPrefs.SetFloat("volume", volume);
    }

    // Sets SFX volume
    public void SetSfxVolume(float volume){
        mainMixer.SetFloat("SFX", volume);

        //PlayerPrefs.SetFloat("SFX", volume);
    }

    // Loads saved volume
    /*private void LoadVolume(){
        if(PlayerPrefs.HasKey("SFX")){
            sfxControl.value = PlayerPrefs.GetFloat("SFX");
        }
        if(PlayerPrefs.HasKey("volume")){
            mainMixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        }  
    }*/

    // Sets brightness
    public void AdjustBrightness(float BrightnessValue){
        for(int i = 0; i < spriteRenderers.Length; i++){
            spriteRenderers[i].color = new Color(BrightnessValue, BrightnessValue, BrightnessValue, spriteRenderers[i].color.a);
        }
    }

    // Sets game to fullscreen
    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }
}

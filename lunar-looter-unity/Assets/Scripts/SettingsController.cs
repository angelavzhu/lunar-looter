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

    private SpriteRenderer[] spriteRenderers;

    // Sets all settings to saved data if changes were made
    public void Start(){
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        /*if(PlayerPrefs.HasKey("volume")){
            LoadVolume();
        }
        else{
            SetVolume();
            SetSfxVolume();
        }*/
    }

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    // Sets game volume
    public void SetVolume(){
        float volume = masterControl.value;
        mainMixer.SetFloat("volume", Mathf.Log10(volume)*20);

        //PlayerPrefs.SetFloat("volume", volume);
    }

    // Sets SFX volume
    public void SetSfxVolume(){
        float volume = sfxControl.value;
        mainMixer.SetFloat("SFX", Mathf.Log10(volume)*20);

        //PlayerPrefs.SetFloat("SFX", volume);
    }

    // Loads saved volume
    /*private void LoadVolume(){
        masterControl.value = PlayerPrefs.GetFloat("Volume");
        sfxControl.value = PlayerPrefs.GetFloat("SFX");

        SetVolume();
        SetSfxVolume();
    }*/

    // Sets brightness
    public void AdjustBrightness(float BrightnessValue){
        for(int i = 0; i < spriteRenderers.Length; i++){
            spriteRenderers[i].color = new Color(BrightnessValue, BrightnessValue, BrightnessValue, spriteRenderers[i].color.a);
        }

        //PlayerPrefs.SetFloat("Brightness", BrightnessValue);
    }

    // Sets game to fullscreen
    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }
}

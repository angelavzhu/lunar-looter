using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    public AudioMixer mainMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    public void Start(){
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
    }

    public void setVolume(float volume){
        mainMixer.SetFloat("volume", volume);
    }

    public void setGraphicsQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsBack : MonoBehaviour
{
    public void Click(){
        SceneManager.LoadScene(SceneController.instance.getScene());
    }
}

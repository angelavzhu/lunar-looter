using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private Scene prevScene;

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        TimeController.instance.BeginTimer();
        prevScene = SceneManager.GetActiveScene();
    }

    public string getScene(){
        return prevScene.name;
    }

    
}

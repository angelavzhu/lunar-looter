using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code for making certain gameobjects appear on or off screen from usier interaction
public class EnableOrDisable : MonoBehaviour
{
    [SerializeField] private GameObject element;

    // Element appears or disappears when user clicks button
    void Update(){
        OpenTablet();
    }

    public void Clicked(){
        if(element.activeInHierarchy){
            element.SetActive(false);
        }
        else{
            element.SetActive(true);
        }
    }

    private void OpenTablet(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(element.activeInHierarchy){
                element.SetActive(false);
            }
            else{
                element.SetActive(true);
            }
        }
    }
}

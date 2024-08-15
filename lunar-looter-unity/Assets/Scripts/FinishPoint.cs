using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    public GameObject winScreen;
    [SerializeField] private Transform player;

    // If player reaches finishing point, goes to the scene for the next level and unlocks that level
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            // win
            UnlockNewLevel();
            Time.timeScale = 0;
            winScreen.SetActive(true);
            player.gameObject.GetComponent<PlayerControl>().enabled = false;
        }
    }

    // The next level is unlocked and its level number and build index are stored
    private void UnlockNewLevel() {
        if(SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("ReachedIndex")) {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }

    }


}

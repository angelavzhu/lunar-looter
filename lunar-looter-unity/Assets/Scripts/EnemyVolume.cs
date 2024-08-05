using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVolume : MonoBehaviour
{
    // Player object
    [SerializeField] private Transform listenerTransform;
    // Enemy audio source
    [SerializeField] private AudioSource audioSource;
    // Minimum distance player can hear enemy from
    private float minDist = 1;
    // Maximum distance player can hear enemy from
    private float maxDist = 5;

    // As player gets farther or closer to enemy, changes volume of enemy sfx
    void Update()
    {
        float dist = Vector3.Distance(transform.position, listenerTransform.position);

        if(dist < minDist)
        {
            audioSource.volume = 1;
        }
        else if(dist > maxDist)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1 - ((dist - minDist) / (maxDist - minDist));
        }
    }
}

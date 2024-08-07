using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A question mark object which alerts the player if the enemy has noticed them.
/// </summary>
public class Question : MonoBehaviour
{
    // the renderer for the question mark
    private SpriteRenderer render;

    // the enemy that the question mark belongs to
    [SerializeField] EnemyControl enemy;

    // the height the question mark should be at
    [SerializeField] float height;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.Noticed() == true) {
            Debug.Log("question rendered");
            transform.position = enemy.transform.position + new Vector3(0, height, 0);
            render.enabled = true;
        } else {
            render.enabled = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyControl : MonoBehaviour
{
    // starting point enemy moves to
    public Transform firstPos;

    // next point enemy moves to
    public Transform secondPos;

    //whether the enemy is chasing the player
    private Boolean isChasing;

    //timer for how long the player is outside of the FOV range
    private int timer;

    // point enemy currently moves towards
    [SerializeField] private Transform targetPos;

    // how fast enemy moves
    [SerializeField] private float speed;

    // direction enemy viewcones point towards
    private Vector2 aimDirection;
    

    [SerializeField] private EnemyFieldOfView fovWide;
    [SerializeField] private EnemyFieldOfView fovNarrow;


    void Start()
    {
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
        isChasing = false;
    }

    // Updates direction of vision cones and vision cone origins each origin
    void LateUpdate()
    {
        fovWide.SetAim(aimDirection);
        fovWide.SetOrigin(transform.position);

        fovNarrow.SetAim(aimDirection);
        fovNarrow.SetOrigin(transform.position);
        Move();
    }


// Enemy moves back and forth from one position to another and changes aim direction based on
// which point enemy moves towards.
    private void Move(){
        if(Vector2.Distance(transform.position, firstPos.position) < 0.01f){
            targetPos = secondPos;
            //aimDirection = new Vector2(targetPos.position.x, 0);
        }
        if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
            //aimDirection = new Vector2(targetPos.position.x, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed*Time.deltaTime);
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
        
    }

    // Handles when the player is seen by the enemy
    public void seePlayer(GameObject player){
        isChasing = true;
        Vector2 chaseDir = player.transform.position;
        transform.position = Vector2.Lerp(transform.position, chaseDir, 1);
        Debug.Log("chase");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EnemyControl : MonoBehaviour
{
    // starting point enemy moves to
    public Transform firstPos;

    // next point enemy moves to
    public Transform secondPos;

    //whether the enemy is chasing the player
    private Boolean isChasing;

    // the player (for chasing)
    private Transform Player;

    // how long the enemy has been chasing the player after the player has left the view range
    private float timer;

    // the max time the enemy should chase the player after leaving the view range
    public float maxTime;

    // whether the enemy is colliding with anything
    private Boolean collide;


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
        Player = GameObject.FindWithTag("Player").transform;
        timer = 0f;
        collide = false;

        // Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Player.GetComponent<Collider2D>(), true);
    }

    // Updates direction of vision cones and vision cone origins and checks for collisions
    void LateUpdate()
    {
        fovWide.SetAim(aimDirection);
        fovWide.SetOrigin(transform.position);

        fovNarrow.SetAim(aimDirection);
        fovNarrow.SetOrigin(transform.position);


        if(isChasing && !collide){ 
            chasePlayer();
        } else if (!collide){
            Move();
        }

    }

    //Method to stop from moving when collide with something
    //TODO: need to change behavior for when hit wall to extrapolate 
    //closest direction to player and move that direction
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = UnityEngine.Vector2.zero;
        collide = true;
    }

    //Method to stop from moving when collide with something
    void OnCollisionExit2D(Collision2D collision)
    {
        collide = false;
    }

// Enemy moves back and forth from one position to another and changes aim direction based on
// which point enemy moves towards.
    private void Move(){
        if(Vector2.Distance(transform.position, firstPos.position) < 0.01f){
            targetPos = secondPos;
        }
        if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
        
    }

    public void seePlayer(Boolean see){
        if(see){
            //see the player
            isChasing = see;
        } else {
            //don't see the player
            if(isChasing){
                outOfRange();
            }
        }
    }

    // Handles when the player is seen by the enemy
    public void chasePlayer(){
        if (Vector3.Distance(transform.position, Player.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
        aimDirection = new Vector2(Player.position.x - transform.position.x, Player.position.y - transform.position.y);
    }

    // Handles when the player is out of the enemy FOV. The enemy will continue to chase the player
    // up to a certain time limit, then return to its original movement pattern.
    public void outOfRange(){
        // Debug.Log(timer);
        timer += Time.deltaTime;
        if(timer > maxTime) {
            isChasing = false;
            timer = timer - maxTime;
        }
    }
}

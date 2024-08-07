using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeepingControl : EnemyControl
{
    //weeping: blind and slow, good hearing
    // starting point enemy moves to
    public Transform firstPos;

    // next point enemy moves to
    public Transform secondPos;

    //whether the enemy is chasing the player
    private Boolean isChasing;

    // the player (for chasing)
    private Transform playerTransform;

    // how long the enemy has been chasing the player after the player has left the view range
    private float chaseDuration;

    // the max time the enemy should chase the player after leaving the view range
    [SerializeField] public float chaseTime;

    // whether the enemy is colliding with anything
    private Boolean collide;

    // point enemy currently moves towards
    [SerializeField] private Transform targetPos;

    // how fast enemy moves
    [SerializeField] private float speed;

    // direction enemy viewcones point towards
    private Vector2 aimDirection;

    // back FOV (hearing)
    [SerializeField] private EnemyFOV fov;

    [SerializeField] private PlayerControl player;

    // Audiosources
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject attack;

    void Start()
    {
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
        isChasing = false;
        playerTransform = GameObject.FindWithTag("Player").transform;
        chaseDuration = 0f;
        collide = false;
        idle.SetActive(true);
        attack.SetActive(false);
    }

    // Updates direction of vision cones and vision cone origins and checks for collisions
    void LateUpdate()
    {
        fov.SetAim(aimDirection);
        fov.SetOrigin(transform.position);

        if(isChasing && !collide){ 
            ChasePlayer();
            idle.SetActive(false);
            attack.SetActive(true);
        } else if (!collide){
            Move();
            idle.SetActive(true);
            attack.SetActive(false);
        }

    }

    //Method to stop from moving when collide with something
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = UnityEngine.Vector2.zero;
        collide = true;
    }

    //Method to stop from moving when collide with something
    protected override void OnCollisionExit2D(Collision2D collision)
    {
        collide = false;
    }

// Enemy moves back and forth from one position to another and changes aim direction based on
// which point enemy moves towards.
    void Move(){
        if(Vector2.Distance(transform.position, firstPos.position) < 0.01f){
            targetPos = secondPos;
        }
        if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
        
    }

    // Control what the enemy does when the player enters the enemy FOV
    public override void SeePlayer(Boolean see){
        Boolean moving = player.isMoving();

        if(see && moving){
            //see the player
            isChasing = see;
        } else {
            //don't see the player
            if(isChasing){
                OutOfRange();
            }
        }
    }

    // Handles when the player is seen by the enemy
    protected override void ChasePlayer(){
        if (Vector3.Distance(transform.position, playerTransform.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
        aimDirection = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
    }

    // Handles when the player is out of the enemy FOV. The enemy will continue to chase the player
    // up to a certain time limit, then return to its original movement pattern.
    protected override void OutOfRange(){
        chaseDuration += Time.deltaTime;
        if(chaseDuration > chaseTime) {
            isChasing = false;
            chaseDuration = chaseDuration - chaseTime;
        }
    }

    public void Destroy(){
        firstPos = null;
        secondPos = null;
        playerTransform = null;
        targetPos = null;
        fov = null;
    }
}

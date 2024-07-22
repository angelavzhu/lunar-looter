using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class ThornControl : EnemyControl
{
    //thorn: rotate

     // starting point enemy moves to
    public Transform firstPos;

    // next point enemy moves to
    public Transform secondPos;

    //whether the enemy is chasing the player
    private Boolean isChasing;

    // the player (for chasing)
    private Transform Player;

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

    // the amount of degrees the FOV rotates
    [SerializeField] private float degrees;

    // how fast the FOV rotates
    [SerializeField] private float rotateSpeed;

    // direction enemy viewcones point towards
    private Vector2 aimDirection;
    
    [SerializeField] private EnemyFOV fov;
    [SerializeField] private EnemyFOV fovBack;
    [SerializeField] private EnemyFOV fovPeriph;


    void Start()
    {
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
        isChasing = false;
        Player = GameObject.FindWithTag("Player").transform;
        chaseDuration = 0f;
        collide = false;
    }

    // Updates direction of vision cones and vision cone origins and checks for collisions
    void LateUpdate()
    {
        fovPeriph.SetAim(aimDirection);
        fovPeriph.SetOrigin(transform.position);

        fov.SetAim(aimDirection);
        fov.SetOrigin(transform.position);

        fovBack.SetAim(-aimDirection);
        fovBack.SetOrigin(transform.position);

        if(isChasing && !collide){ 
            ChasePlayer();
        } else if (!collide){
            Move();
            Rotate();
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

    // rotates the enemy's FOV
    //TODO: works until the FOV is changed as the enemy moves (turns), need way to track and reset angle?
    private void Rotate(){ 
       Vector2 newDir = aimDirection + AngleToVector(rotateSpeed);
       if(VectorToAngle(newDir) > degrees || VectorToAngle(newDir) < -degrees) {
        rotateSpeed = -rotateSpeed;
       }
       aimDirection += AngleToVector(rotateSpeed);
       Debug.Log("rotated " + aimDirection);
    }

    // Control what the enemy does when the player enters the enemy FOV
    public override void SeePlayer(Boolean see){
        if(see){
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
        if (Vector3.Distance(transform.position, Player.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
        aimDirection = new Vector2(Player.position.x - transform.position.x, Player.position.y - transform.position.y);
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

     // Converts an angle to a Vector3
    // angle = float value for degree of angle
    private Vector2 AngleToVector(float angle){
        float angleRadius = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRadius), Mathf.Sin(angleRadius));
    }

    //Converts a Vector3 to an angle (float)
    private float VectorToAngle(Vector2 vector) {
        vector = vector.normalized;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if(angle < 0) angle += 360;
        
        return angle;
    }

    public void Destroy(){
        firstPos = null;
        secondPos = null;
        Player = null;
        targetPos = null;
        fovPeriph = null;
        fov = null;
        fovBack = null;
    }
}

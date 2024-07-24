using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;

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

    // how long the FOV is flipping (turn)
    float flipTime = 0f;

    // whether the enemy is colliding with anything
    private Boolean collide;

    // whether the enemy is turning 
    private Boolean turning;

    // point enemy currently moves towards
    [SerializeField] private Transform targetPos;

    // how fast enemy moves
    [SerializeField] private float speed;

    // the amount of degrees the FOV rotates (0-90 degrees which is mirrored on the other side)
    [SerializeField] private float degrees;

    // how fast the FOV rotates
    [SerializeField] private float rotateSpeed;

    // direction enemy viewcones point towards
    private Vector2 aimDirection;

    // where the enemy is moving (different that aimDirection because FOV is rotating)
    private Vector2 moveDirection;
    
    [SerializeField] private EnemyFOV fov;
    [SerializeField] private EnemyFOV fovBack;
    [SerializeField] private EnemyFOV fovPeriph;


    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        moveDirection = targetPos.position - transform.position;
        aimDirection = moveDirection;
        isChasing = false;
        collide = false;
        turning = false;
        chaseDuration = 0f;
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
            turning = true;
        }
        else if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
            turning = true;

        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        moveDirection = targetPos.position - transform.position;    
    }

    // rotates the enemy's FOV
    //TODO: works until the FOV is changed as the enemy moves (turns), need way to track and reset angle?
    private void Rotate(){ 
        //must rotate the axis: 90 degrees is always where the enemy is aiming
       float newAim = VectorToAngle(aimDirection) + rotateSpeed;
       float new180 = VectorToAngle(moveDirection) + degrees;
       float newZero = VectorToAngle(moveDirection) - degrees;
       if (turning) {
            newAim = VectorToAngle(aimDirection) + rotateSpeed * 4;
            // Debug.Log(newAim + " " + rotateSpeed);
            if(Math.Abs(newAim - VectorToAngle(moveDirection)) < degrees) {
                turning = false;
                flipTime -= 1;
            }
       }
       else if(newAim > new180 || newAim < newZero) {
        //FOV out of bounds, rotate the other way
        rotateSpeed = -rotateSpeed;
        newAim = VectorToAngle(aimDirection) + rotateSpeed;
       }
       aimDirection = AngleToVector(newAim);
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

    //Converts a Vector3 to an angle (float, degrees)
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

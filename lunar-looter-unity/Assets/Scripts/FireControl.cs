using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireControl : EnemyControl
{
    // a reference to the player
    private Transform Player;

    // the speed the enemy moves at
    [SerializeField] public float speed;

    // the max time the enemy should chase the player after leaving the view range
    [SerializeField] public float chaseTime;

    // the max time the fire guardian rests (FOV off)
    [SerializeField] public float awakeTime;

    private Boolean sleeping;

    // the enemy's FOV
    [SerializeField] private EnemyFOV fov;

    // whether the enemy is chasing the player;
    private Boolean isChasing;

    // whether the enemy is colliding with anything
    private Boolean collide;

    // how long the enemy has been chasing the player after the player has left the view range
    private float chaseDuration;

    private float awakeDuration;

    // Audio for enemy
    [SerializeField] private AudioSource idle;
    [SerializeField] private AudioSource attack;


    // Start is called before the first frame update
    void Start()
    {
        isChasing = false;
        chaseDuration = 0f;
        awakeDuration = 0f;
        collide = false;
        sleeping = false;
        Player = GameObject.FindWithTag("Player").transform;
        //idle.enabled = true;
        //attack.enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        fov.SetOrigin(transform.position);
        if(isChasing && !collide){ 
            ChasePlayer();
        }
        else {
            //attack.enabled = false;
            //idle.enabled = true;
        }
        Sleep();
    }

    public override void SeePlayer(bool see)
    {
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

    private void Sleep(){
        awakeDuration += Time.deltaTime;
        if(awakeDuration > awakeTime) {
            fov.Toggle();
            awakeDuration -= awakeTime;
        }
    }

    protected override void OutOfRange(){
        chaseDuration += Time.deltaTime;
        if(chaseDuration > chaseTime) {
            isChasing = false;
            chaseDuration -= chaseTime;
        }
    }

    protected override void ChasePlayer(){
        if (Vector3.Distance(transform.position, Player.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
            //idle.enabled = false;
            //attack.enabled = true;
        }
    }
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = UnityEngine.Vector2.zero;
        if(!(collision.gameObject.layer == 3 || collision.gameObject.tag == "Enemy")) {
            //don't freeze if collide with wall or other enemy
            collide = true;
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        collide = false;
    }
}

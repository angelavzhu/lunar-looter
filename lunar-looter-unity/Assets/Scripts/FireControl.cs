using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A class for fire spirits, an enemy with a circular sight cone that blinks on and off.
/// </summary>
public class FireControl : EnemyControl
{
    // the state the enemy is in (enumeration below)
    private int state;

    // a reference to the player
    private Transform Player;

    // the speed the enemy moves at
    [SerializeField] public float speed;

    // the max time the enemy should chase the player after leaving the view range
    [SerializeField] public float chaseTime;

    // the max time the fire guardian rests (FOV off)
    [SerializeField] public float awakeTime;

    // the enemy's FOV
    [SerializeField] private EnemyFOV fov;

    [SerializeField] private Transform originalPos;

    // whether the enemy is colliding with anything
    private Boolean collide;

    // how long the enemy has been chasing the player after the player has left the view range
    private float chaseDuration;
    
    // how long the enemy has been awake
    private float awakeDuration;

    // Audio for enemy
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject attack;
    [SerializeField] private AudioSource attackWail;

    // Checks if enemy is chasing player
    private bool isAttacking;


    // Start is called before the first frame update
    void Start()
    {
        state = (int) State.Idle;
        chaseDuration = 0f;
        awakeDuration = 0f;
        collide = false;
        Player = GameObject.FindWithTag("Player").transform;
        idle.SetActive(true);
        attack.SetActive(false);
        isAttacking = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        fov.SetOrigin(transform.position);
        if(state == (int) State.Chasing){ 
            ChasePlayer();
        } else if (state == (int) State.Return) {
            transform.position = Vector2.MoveTowards(transform.position, originalPos.position, speed * Time.deltaTime);
            if((Vector2) transform.position == (Vector2) originalPos.position) {
                state = (int) State.Idle;
            }
        } else {
            attack.SetActive(false);
            idle.SetActive(true);
            Sleep();
        }
    }

    public override void SeePlayer(bool see)
    {
        Debug.Log(see);
        if(see){
            //see the player
            state = (int) State.Chasing;
        } else {
            //don't see the player
            if(state == (int) State.Chasing){
                OutOfRange();
            }
        }
    }

    // Updates whether enemy FOV should be on or off
    private void Sleep(){
        awakeDuration += Time.deltaTime;
        if(awakeDuration > awakeTime) {
            fov.Toggle();
            awakeDuration -= awakeTime;
        }
    }

    // Calculates whether enemy has been chasing player too long
    protected override void OutOfRange(){
        chaseDuration += Time.deltaTime;
        if(chaseDuration > chaseTime) {
            // enemy chase cools down, return to idle
            state = (int) State.Return;
            chaseDuration -= chaseTime;
            isAttacking = false;
        }
    }

    protected override void ChasePlayer(){
        if (Vector3.Distance(transform.position, Player.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
            idle.SetActive(false);
            attack.SetActive(true);
            // Plays wailing sfx once only when enemy starts chasing player
            if(!attackWail.isPlaying && !isAttacking){
                attackWail.Play();
                isAttacking = true;
            }
            
            
        }
    }
    public void Destroy(){
       Player = null;
       originalPos = null;
       fov = null;
       idle = null;
       attackWail = null;
       attack = null;
    }
}
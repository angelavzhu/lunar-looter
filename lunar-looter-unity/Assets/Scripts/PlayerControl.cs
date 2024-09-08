using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // the movement speed of the player
    public float moveSpeed;

    // the x and y position of the player
    float x,y;

    private Boolean moving;

    // the rigidbody of the player
    Rigidbody2D body;

    UnityEngine.Vector2 aimDirection;

    // reference to the FOV
    [SerializeField] private FieldOfView fov;
    // animation controller object
    Animator playerAnimator;
    // Audio source for footsteps
    [SerializeField] private AudioSource footsteps;

    public GameObject restartScreen;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        moving = false;
        footsteps.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        LightToggle();
        Animate();
        SetAimDirection();
        fov.SetAim(aimDirection);
        fov.SetOrigin(transform.position);
    }

    private void SetAimDirection(){
        UnityEngine.Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = new UnityEngine.Vector2(mouse.x - transform.position.x, 
                                    mouse.y - transform.position.y);

    }

    // whether the player is moving
    public Boolean isMoving(){
        return moving;
    }

    // polls input from the keyboard and moves the player
    private void Move(){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if(horizontal == 0 && vertical == 0){
                body.velocity = new UnityEngine.Vector2(0,0);
                moving = false;
                footsteps.enabled = false;
                return;
            }

            x =  horizontal * moveSpeed;
            y =  vertical * moveSpeed;
            
            body.velocity = new UnityEngine.Vector2(x,y);
            moving = true;
            footsteps.enabled = true;
    }

    // Whether the player has toggled the light
    private void LightToggle() {
        if(Input.GetMouseButtonUp(0)) {
            fov.Toggle();
        }
    }

    // Shows death/restart screen if player hits enemy
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Enemy")){
            restartScreen.SetActive(true);
            // body.gameObject.GetComponent<PlayerControl>().enabled = false;
            // body.velocity = new UnityEngine.Vector2(0,0);
            footsteps.enabled = false;
            Time.timeScale = 0;
        }
    }

    // Player animation
    private void Animate(){
        if(body.velocity != UnityEngine.Vector2.zero){
            playerAnimator.SetBool("Walk", true);
            playerAnimator.SetFloat("MovementX", x);
            playerAnimator.SetFloat("MovementY", y);
        }
        else{
            playerAnimator.SetBool("Walk", false);
        }
        
    }
}

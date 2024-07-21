using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // the movement speed of the player
    public float moveSpeed;

    // the x and y position of the player
    float x,y;

    // the rigidbody of the player
    Rigidbody2D body;

    Vector2 aimDirection;

    // reference to the FOV
    [SerializeField] private FieldOfView fov;

    // animation controller object
    Animator playerAnimator;

    public GameObject restartScreen;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        // aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
        Animate();

        SetAimDirection();
        fov.SetAim(aimDirection);
        fov.SetOrigin(transform.position);
    }

    private void SetAimDirection(){
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = new Vector2(mouse.x - transform.position.x, 
                                    mouse.y - transform.position.y);

    }

    private void Move(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(horizontal == 0 && vertical == 0){
            body.velocity = new Vector2(0,0);
            return;
        }

        x =  horizontal * moveSpeed;
        y =  vertical * moveSpeed;
        
        body.velocity = new Vector2(x,y);
    }

    // Shows death/restart screen if player hits enemy
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Enemy")){
            restartScreen.SetActive(true);
            body.gameObject.GetComponent<PlayerControl>().enabled = false;
        }
    }

    // Player animation
    private void Animate(){
        if(body.velocity != Vector2.zero){
            playerAnimator.SetBool("Walk", true);
            playerAnimator.SetFloat("MovementX", x);
            playerAnimator.SetFloat("MovementY", y);
        }
        else{
            playerAnimator.SetBool("Walk", false);
        }
        
    }
}

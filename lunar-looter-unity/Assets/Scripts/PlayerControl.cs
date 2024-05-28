using System;
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

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        // aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        y = Input.GetAxisRaw("Vertical") * moveSpeed;
        body.velocity = new Vector2(x,y);

        SetAimDirection();
        fov.SetAim(aimDirection);
        fov.SetOrigin(transform.position);
    }

    private void SetAimDirection(){
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = new Vector2(mouse.x - transform.position.x, 
                                    mouse.y - transform.position.y);

    }
}

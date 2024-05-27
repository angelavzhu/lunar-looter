using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    float x,y;
    Rigidbody2D body;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        y = Input.GetAxisRaw("Vertical") * moveSpeed;
        body.velocity = new Vector2(x,y);
    }
}

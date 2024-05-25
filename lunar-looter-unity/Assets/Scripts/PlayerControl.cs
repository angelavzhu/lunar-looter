using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    float x,y;
    Rigidbody2D body;
    public GameObject enemy;

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

// When player hits enemy, kills enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")){
            Destroy(enemy);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementTest : MonoBehaviour
{
    public Transform firstPos;
    public Transform secondPos;
    public Transform targetPos;
    public float speed;

// Sets target position to firstPos first
    void Awake()
    {
        targetPos = firstPos;
    }



    void Update()
    {
        Move();
    }


// Enemy moves back and forth from one position to another
    private void Move(){
        if(Vector2.Distance(transform.position, firstPos.position) < 0.01f){
            targetPos = secondPos;
        }
        if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed*Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementTest : MonoBehaviour
{
    public Transform firstPos;
    public Transform secondPos;
    public Transform targetPos;
    [SerializeField] private float distance;
    public float speed;

    void Awake()
    {
        targetPos = firstPos;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }


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

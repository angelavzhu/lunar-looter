using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementTest : MonoBehaviour
{
    // starting point enemy moves to
    public Transform firstPos;

    // next point enemy moves to
    public Transform secondPos;

    // point enemy currently moves towards
    [SerializeField] private Transform targetPos;

    // how fast enemy moves
    [SerializeField] private float speed;

    // direction enemy viewcones point towards
    [SerializeField] private Vector2 aimDirection;

    [SerializeField] private EnemyFieldOfView fovWide;
    [SerializeField] private EnemyFieldOfView fovNarrow;


    // Updates direction of vision cones and vision cone origins each origin
    void LateUpdate()
    {
        fovWide.SetAim(aimDirection);
        fovWide.SetOrigin(transform.position);

        fovNarrow.SetAim(aimDirection);
        fovNarrow.SetOrigin(transform.position);
        Move();
    }


// Enemy moves back and forth from one position to another and changes aim direction based on
// which point enemy moves towards.
    private void Move(){
        if(Vector2.Distance(transform.position, firstPos.position) < 0.01f){
            targetPos = secondPos;
        }
        if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed*Time.deltaTime);
        aimDirection = new Vector2(0, targetPos.position.y);
    }
}

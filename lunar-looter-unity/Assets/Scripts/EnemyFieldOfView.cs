using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    public float fovAngleOuter = 130f;    
    public float fovAngleInner = 90f;
    public Transform fovPointOuter;
    public Transform fovPointInner;
    private float rangeOuter = 4.5f;
    private float rangeInner = 3f;

    public Transform target;


    /* Whenever player is in outer vision cone, alerts enemy but they don't know for sure (prints "seen ???"
    for testing). 

    Whenever player in inner vision cone, alerts enemy and they know (prints "seen !!!" for 
    testing). 
    
    Otherwise, player is safe from enemies (prints "not seen" for testing).*/
    void FixedUpdate() {
        Vector2 dir = target.position - transform.position;
        float angleOuter = Vector3.Angle(dir, fovPointOuter.up);
        float angleInner = Vector3.Angle(dir, fovPointInner.up);
        RaycastHit2D raycastOuter = Physics2D.Raycast(fovPointOuter.position, dir, rangeOuter);
        RaycastHit2D raycastInner = Physics2D.Raycast(fovPointInner.position, dir, rangeInner);

        if(angleOuter < fovAngleOuter/2){
            if(raycastOuter.collider.CompareTag("Player")){
                print("seen ???");
                Debug.DrawRay(fovPointOuter.position, dir, Color.red);
                
                if(angleInner < fovAngleInner/2){
                    if(raycastInner.collider.CompareTag("Player")){
                        print("seen !!!");
                        Debug.DrawRay(fovPointInner.position, dir, Color.blue);
                    }
                }
            }
            else {
                print("Not seen");
            }
        }
        else {
                print("Not seen");
            }
        
    }
}

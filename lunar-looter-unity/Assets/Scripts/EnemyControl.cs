using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class EnemyControl : MonoBehaviour
{
    public virtual void SeePlayer(Boolean see){}
    protected virtual void OutOfRange(){}
    
    // collision todos: don't freeze when touch wall, don't freeze if touched from behind
    //TODO: need to change behavior for when hit wall to extrapolate 
    //closest direction to player and move that direction
    protected virtual void OnCollisionEnter2D(Collision2D collision){}
    protected virtual void OnCollisionExit2D(Collision2D collision){}
    protected virtual void ChasePlayer(){
    }
}

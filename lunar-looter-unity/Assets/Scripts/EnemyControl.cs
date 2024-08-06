using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A superclass used to group all enemies into one useable object for reference.
/// </summary>
[System.Serializable] public class EnemyControl : MonoBehaviour
{
    // changes enemy state based on whether the player is seen.
    // boolean see whether the player is seen (true for yes, false for no)
    public virtual void SeePlayer(Boolean see){}

    // changes enemy state based on whether the player is noticed
    // Vector3 position the position of the player as last seen in the peripheral FOV
    public virtual void NoticePlayer(Boolean see, Vector3 position){}

    // whether the enemy has noticed the player
    public virtual Boolean Noticed(){ return false; }

    // enemy resets to resting state
    protected virtual void OutOfRange(){}
    
    // collision actions for the enemy
    // Collision 2D the object being collided with
    protected virtual void OnCollisionEnter2D(Collision2D collision){}
    protected virtual void OnCollisionExit2D(Collision2D collision){}
    
    // enemy actions in chase state
    protected virtual void ChasePlayer(){
    }
}

// ==================================================
    // enemy state enumeration:
    // 0 = chasing
    // 1 = idle
    // 2 = notice player
    // 3 = return to original position
// ==================================================
 public enum State {
        Chasing,
        Notice,
        Idle,
        Return
    }



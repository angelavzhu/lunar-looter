using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// A class for mobs/ghost enemies, which move in a set pattern and have static triangular sight 
/// cones with peripheral vision.
/// </summary>
public class MobControl : EnemyControl
{
    // the state that the enemy is in
    private int state;
    // starting point enemy moves to
    public Transform firstPos;

    // next point enemy moves to
    public Transform secondPos;

    // the player (for chasing)
    private Transform Player;

    // how long the enemy has been chasing the player after the player has left the view range
    private float chaseDuration;

    // the max time the enemy should chase the player after leaving the view range
    [SerializeField] public float chaseTime;

    // whether the enemy is colliding with anything
    private Boolean collide;

    // point enemy currently moves towards
    [SerializeField] private Transform targetPos;

    // how fast enemy moves
    [SerializeField] private float speed;

    // direction enemy viewcones point towards
    private Vector2 aimDirection;
    private float xCenter;
    private float yCenter;
    
    //how fast the enemy rotates to see the player when noticed
    [SerializeField] private float rotateSpeed;

    [SerializeField] private PeripheralVision fovPeriph;
    [SerializeField] private EnemyFOV fov;
    [SerializeField] private EnemyFOV fovBack;

    // Audiosources
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject attack;

    void Start()
    {
        aimDirection = targetPos.position - transform.position;
        Player = GameObject.FindWithTag("Player").transform;
        chaseDuration = 0f;
        collide = false;
        state = (int) State.Idle;
        xCenter = transform.position.x;
        yCenter = transform.position.y;
        idle.SetActive(true);
        attack.SetActive(false);
        // fillCells();
    }

    // Updates direction of vision cones and vision cone origins and checks for collisions
    void LateUpdate()
    {
        xCenter = transform.position.x;
        yCenter = transform.position.y;
        
        fovPeriph.SetAim(aimDirection);
        fovPeriph.SetOrigin(transform.position);

        fov.SetAim(aimDirection);
        fov.SetOrigin(transform.position);

        fovBack.SetAim(-aimDirection);
        fovBack.SetOrigin(transform.position);

        if(state == (int) State.Chasing){ 
            ChasePlayer();
            idle.SetActive(false);
            attack.SetActive(true);
        } else if (state == (int) State.Return){
            transform.position = Vector2.MoveTowards(transform.position, firstPos.position, speed * Time.deltaTime);
            state = (int) State.Idle;
        } else {
            Move();
            idle.SetActive(true);
            attack.SetActive(false);
        }

    }

// Enemy moves back and forth from one position to another and changes aim direction based on
// which point enemy moves towards.
    void Move(){
        if(Vector2.Distance(transform.position, firstPos.position) < 0.01f){
        targetPos = secondPos;
        }
        if(Vector2.Distance(transform.position, secondPos.position) < 0.01f){
            targetPos = firstPos;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        aimDirection = new Vector2(targetPos.position.x - transform.position.x, targetPos.position.y - transform.position.y);
    }

    // Control what the enemy does when the player enters the enemy FOV
    public override void SeePlayer(Boolean see){
        if(see){
            //see the player
            state = (int) State.Chasing;
        } else {
            //don't see the player
            if(state == (int) State.Chasing){
                OutOfRange();
            }
        }
    }

    /// <summary>
    /// Handles whether the player is noticed but not fully seen by the enemy.
    /// If seen, slowly turns towards the last position that the player was noticed.
    /// </summary>
    /// <param name="see"></param> whether the player was noticed
    /// <param name="pos"></param> the last position the player was noticed, scaled on world axis
     public override void NoticePlayer(Boolean see, Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x - xCenter, pos.y - yCenter, pos.z);
        if(state != (int) State.Chasing) {
            if(see) {
                
                    state = (int) State.Notice;
                    // aimDirection = aimDirection + rotateSpeed * (Vector2) pos.normalized;
                    aimDirection = Vector3.Lerp(transform.position, newPos, rotateSpeed/10);
            } else {
                state = (int) State.Idle;
            }
        }
    }

    public override bool Noticed()
    {
        return (state == (int) State.Notice);
    }


    // Handles when the player is seen by the enemy
    protected override void ChasePlayer(){
        if (Vector3.Distance(transform.position, Player.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
        aimDirection = new Vector2(Player.position.x - transform.position.x, Player.position.y - transform.position.y);
        // Stack<Vector2> path = FindPath();
        // if(path != null) {
        //     while(transform.position.Equals(path.Peek())) {
        //         path.Pop();
        //     }
        //     transform.position = path.Peek()/2;
        //     aimDirection = path.Peek() - (Vector2)transform.position;
        //     Debug.Log(transform.position);
        // }
    }

    // Handles when the player is out of the enemy FOV. The enemy will continue to chase the player
    // up to a certain time limit, then return to its original movement pattern.
    protected override void OutOfRange(){
        chaseDuration += Time.deltaTime;
        if(chaseDuration > chaseTime) {
            state = (int) State.Return;
            chaseDuration = chaseDuration - chaseTime;
        }
    }

     //Method to stop from moving when collide with something
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = UnityEngine.Vector2.zero;
        if(!(collision.gameObject.layer == 3 || collision.gameObject.tag == "Enemy")) {
            //don't freeze if collide with wall or other enemy
            collide = true;
        }
    }

    //Method to stop from moving when collide with something
    protected override void OnCollisionExit2D(Collision2D collision)
    {
        collide = false;
    }

    public void Destroy(){
        firstPos = null;
        secondPos = null;
        Player = null;
        targetPos = null;
        fovPeriph = null;
        fov = null;
        fovBack = null;
        attack = null;
        idle = null;
    }
}

//     /** =======================================================
//         finding path
//         ========================================================*/

//     Vector2[,] cells = new Vector2 [100,100];
    
//     private void fillCells() {
//         for (int i = 0; i < 100; i++) {
//             for(int j = 0; j < 100; j++){
//                 cells[i,j] = new Vector2(i,j);
//             }
//         }
//     }
    
//     private Stack<Vector2> GetPath(Vector2 end, Dictionary<Vector2, Vector2> reversePath){
//         Stack<Vector2> path = new Stack<Vector2>();
//         Vector2 current = end;
//         while(reversePath[current] != current) {
//             path.Push(current);
//             current = reversePath[current];
//         }
//         return path;
//     }

//     private Stack<Vector2> FindPath() {
//         Vector2 playerPos = Player.position;
//         Dictionary<Vector2,Vector2> path = new Dictionary<Vector2,Vector2>();
//         Queue<Vector2> frontier = new Queue<Vector2>();
//         int tileSize = 1;
//         // Debug.DrawLine(transform.position, Vector2.zero, Color.blue);
//         path.Add(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y));
//         frontier.Enqueue(new Vector2(transform.position.x, transform.position.y));
        
//         while(frontier.Count > 0){
//             Vector2 temp = frontier.Dequeue();
//             Vector2[] neighbors = {new Vector2(temp.x+tileSize, temp.y), new Vector2(temp.x-tileSize, temp.y), 
//             new Vector2(temp.x, temp.y+tileSize), new Vector2(temp.x, temp.y-tileSize)};
//             //must check if the neighbor is in bounds
//             foreach (Vector2 x in neighbors)
//             {
//                 if(!path.ContainsKey(x)){
//                     if(Physics2D.OverlapPoint(x) == null){
//                         // Debug.Log("found plausible tile");
//                         // nothing on the point
//                         path[x] = temp;
//                         // Debug.Log("X-axis " + Mathf.Abs(x.x-playerPos.x) + " " + "Y-axis " + Mathf.Abs(x.y-playerPos.y));
//                         if(Mathf.Abs(x.x-playerPos.x) < tileSize && Mathf.Abs(x.y-playerPos.y) < tileSize) {
//                             Debug.Log("path found!");
//                             return GetPath(x, path);
//                         }
//                         // frontier.Enqueue(x);
//                     }
//                 }
//             }
//         }
//         // Debug.Log("done looping");
//         return null;
//     }
// }
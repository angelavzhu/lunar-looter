using System;
using Unity.VisualScripting;
using UnityEngine;

// Based on code by Code Monkey
public class PeripheralVision : MonoBehaviour
{
    // the mesh of the FOV
    private Mesh mesh;
    
    // the origin of the FOV
    private Vector3 origin;

    // the starting angle of the FOV
    private float startingAngle;

    // the location of the player as seen in peripheral
    private Vector3 playerLoc;

    // angle of the sightcone for the enemy (wider/narrower sight line)
    [SerializeField] private float fov;
    
    // how far enemy can see
    [SerializeField] private float viewDistance;

    // layermask for all the objects which should block the FOV (walls)
    [SerializeField] private LayerMask layermask;

    // reference to the enemy
    [SerializeField] private EnemyControl enemy;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        
        //origin of sight cone
        origin = Vector3.zero;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    //called every frame: AFTER player update is called
    void LateUpdate(){ 
        // peripheral FOV is invisible
        CreateFOV();
        GetComponent<MeshRenderer>().enabled = false;
    }


    // Makes/updates mesh for the field of view
    private void CreateFOV(){
        //number of rays for raycasting
        int rayCount = 50;

        // turning angle: starts at 0 and goes down
        float angle = startingAngle;

        //how much the angle changes as you turn
        float angleIncrease = fov/rayCount;

        //start at origin (1 vertex) + rayCount rays
        // num rays dont count 0 so have to add 1 for 0 ray (+2 total)
        Vector3[] vertices = new Vector3[rayCount + 2];

        int[] triangles = new int[rayCount * 3];

        // origin
        vertices[0] = origin;

        //set vertices in correct position (angle) and generate triangles
        //vector 0 is a horizontal line
        //forms 2 triangles
        int triangleIndex = 0;
        //need to see for every vertex if it sees the player: if all don't see, then false
        Boolean noticePlayer = false;

        for(int i = 1; i <= rayCount + 1; i++){
            //instead of a set position, each vertex should be raycasted
            RaycastHit2D ray = Physics2D.Raycast(origin, AngleToVector(angle), viewDistance, layermask);
            Collider2D hit = ray.collider;
            
            //assume nothing blocking FOV
            Vector3 vertex = origin + AngleToVector(angle) * viewDistance;

            if(hit != null){
                 //something blocking the FOV: set vertex to where it was blocked
                if (hit.tag == "Player") {
                    //player enters vision
                    noticePlayer = true;
                    playerLoc = ray.point;
                } else {
                    // see a wall: stop sight cone at wall
                    vertex = ray.point;
                }
            }
           
            vertices[i] = vertex;

            if(i-1 > 0){
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = i - 1;
                triangles[triangleIndex + 2] = i;
                triangleIndex += 3;
            }
        angle -= angleIncrease;
        }

        //process whether enemy saw the player
        enemy.NoticePlayer(noticePlayer, playerLoc);


        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();        
    }

    // Sets the origin of the FOV
    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }

    // Sets the angle for the FOV
    public void SetAim(Vector2 direction){
        startingAngle = VectorToAngle(direction) + (fov / 2f);
    }

    // Converts an angle to a Vector3
    // angle = float value for degree of angle
    private Vector3 AngleToVector(float angle){
        float angleRadius = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRadius), Mathf.Sin(angleRadius));
    }

    //Converts a Vector3 to an angle (float)
    private float VectorToAngle(Vector3 vector) {
        vector = vector.normalized;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if(angle < 0) angle += 360;
        
        return angle;
    }

}

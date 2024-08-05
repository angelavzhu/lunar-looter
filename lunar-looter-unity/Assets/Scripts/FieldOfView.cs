using System;
using Unity.VisualScripting;
using UnityEngine;

// Based on code by Code Monkey
public class FieldOfView : MonoBehaviour
{
    // the mesh of the FOV
    private Mesh mesh;
    
    // the origin of the FOV
    private Vector3 origin;

    // the starting angle of the FOV
    private float startingAngle;

    // angle of the sightcone for the player (wider/narrower sight line)
    private float fov;

    // layermask for all the objects which should block the FOV (walls)
    [SerializeField] private LayerMask layermask;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        fov = 80f;
        //origin of sight cone
        origin = Vector3.zero;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    //called every frame: AFTER player update is called
    void LateUpdate(){ 
        // NOTE: FOV is blocked by objects in the layer "walls", must change this in editor. 
        CreateFOV();
    }


    // Makes/updates mesh for the field of view
    private void CreateFOV(){
        //number of rays for raycasting
        int rayCount = 50;

        // turning angle: starts at 0 and goes down
        float angle = startingAngle;

        //how much the angle changes as you turn
        float angleIncrease = fov/rayCount;

        //how far the character can see
        float viewDistance = 6f;

        //start at origin (1 vertex) + 2 rays
        // num rays dont count 0 so have to add 1 for 0 ray
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];

        // Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        // origin
        vertices[0] = origin;

        //set vertices in correct position (angle) and generate triangles
        //vector 0 is a horizontal line
        //forms 2 triangles
        int triangleIndex = 0;
        for(int i = 1; i <= rayCount + 1; i++){
            //instead of a set position, each vertex should be raycasted
            RaycastHit2D ray = Physics2D.Raycast(origin, AngleToVector(angle), viewDistance, layermask);
            Vector3 vertex;

            if(ray.collider == null){
                //nothing blocking the FOV
                vertex = origin + AngleToVector(angle) * viewDistance;
            } else {
                //something blocking the FOV: set vertex to where it was blocked
                vertex = ray.point;
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();        
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

    // Sets the origin of the FOV
    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }

    // Sets the angle for the FOV
    public void SetAim(Vector2 direction){
        startingAngle = VectorToAngle(direction) + (fov / 2f);
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FieldOfView : MonoBehaviour
{
    [Header("Collide With")]
    [SerializeField] LayerMask layerMask;
    [Header("FOV Details")]
    [SerializeField] float fov = 90f;
    [SerializeField] int rayCount = 2;
    [SerializeField] float startingAngle = 0f;
    [SerializeField] float viewDistance = 50f;
    Vector3 origin;
    Mesh mesh;
    bool flashlightOn = false;
    
    // Based on https://www.youtube.com/watch?v=CSeUMTaNFYk
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
    }
    
    void LateUpdate()
    {
        if (flashlightOn)
        {
            UpdateMesh();
        }
    }
    
    void UpdateMesh()
    {
        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null)  // Did not hit anything.
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        angle = 0;
    }
    
    // Converts an angle in degrees to a Vector3 bounded from -1 to +1.
    Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    
    float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }
    
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
    
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVector(aimDirection) + fov / 2f;
    }
    
    public void SetFlashlight(bool flashlight) { flashlightOn = flashlight; }
    public bool GetFlashlight() { return flashlightOn; }
}

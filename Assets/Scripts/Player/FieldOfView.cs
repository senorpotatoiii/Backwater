using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// <para>
/// Uses meshes to create a toggleable flashlight on the player that collides
/// with the environment.
/// </para>
/// <see href="https://www.youtube.com/watch?v=CSeUMTaNFYk"/>
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{
    [Header("Collide With")]
    [SerializeField] private LayerMask _layerMask;
    [Header("FOV Details")]
    [SerializeField] private float _fov = 90f;
    [SerializeField] private int _rayCount = 2;
    [SerializeField] private float _startingAngle = 0f;
    [SerializeField] private float _viewDistance = 50f;
    private Vector3 _origin;
    private Mesh _mesh;
    private bool _flashlightOn;
    
    public void Flashlight(InputAction.CallbackContext context)
    {
        GetComponent<MeshRenderer>().enabled = _flashlightOn = !_flashlightOn;
    }
    
    private void Start()
    {
        _flashlightOn = false;
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _origin = Vector3.zero;
    }
    
    private void LateUpdate()
    {
        if (_flashlightOn)
        {
            UpdateMesh();
        }
    }
    
    private void UpdateMesh()
    {
        Vector3[] vertices = new Vector3[_rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[_rayCount * 3];

        float angle = _startingAngle;
        float angleIncrease = _fov / _rayCount;

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= _rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, GetVectorFromAngle(angle), _viewDistance, _layerMask);
            if (raycastHit2D.collider == null)  // Did not hit anything.
            {
                vertex = _origin + GetVectorFromAngle(angle) * _viewDistance;
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

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        angle = 0;
    }
    
    // Converts an angle in degrees to a Vector3 bounded from -1 to +1.
    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    
    private float GetAngleFromVector(Vector3 dir)
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
        _origin = origin;
    }
    
    public void SetAimDirection(Vector3 aimDirection)
    {
        _startingAngle = GetAngleFromVector(aimDirection) + _fov / 2f;
    }
}

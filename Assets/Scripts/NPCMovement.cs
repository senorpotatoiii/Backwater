using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Rigidbody2D rb;
    Node currentNode;
    List<Node> path = new List<Node>();
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        currentNode = nodes[Random.Range(0, nodes.Length)];
    }
    
    void Update()
    {
        CreatePath();
    }
    
    void CreatePath()
    {
        if (path.Count > 0)
        {
            int x = 0;

            MoveTo(path[x].transform.position);
            
            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
        else
        {
            Node[] nodes = FindObjectsOfType<Node>();
            while (path == null || path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, nodes[Random.Range(0, nodes.Length)]);
            }
        }
    }
    
    public void MoveTo(Vector2 destination)
    {
        Vector2 direction = (destination - new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.velocity = direction * speed;
        Debug.Log($"Direction: {direction}. Velocity: {rb.velocity.magnitude}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>NodeSpawner</c> creates a grid of <c>Node</c> objects and
/// sets their connections automatically.
/// </summary>
public class NodeSpawner : MonoBehaviour
{
    [SerializeField] Node prefab;
    [SerializeField] int rows = 3;
    [SerializeField] int collumns = 3;
    [SerializeField] float spacing = 1f;
    List<Node> nodes = new List<Node>();
    
    void Start()
    {
        CreateNodes();
        CreateConnections();
    }
    
    /// <summary>
    /// Creates a grid of <c>Node</c> objects, expanding right and down.
    /// </summary>
    void CreateNodes()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {
                Vector3 newPosition = new Vector3(transform.position.x + (j * spacing),
                                                    transform.position.y - (i * spacing));
                Node newNode = Instantiate(prefab, newPosition, Quaternion.identity, transform);
                nodes.Add(newNode);
            }
        }
    }

    /// <summary>
    /// Adds connections for previously created <c>Node</c> objects in
    /// <c>CreateNodes</c>.
    /// <see cref="CreateNodes" />
    /// </summary>
    void CreateConnections()
    {
        foreach (Node node in nodes)
        {
            bool leftEdge = nodes.IndexOf(node) % collumns == 0;
            bool rightEdge = nodes.IndexOf(node) % collumns == collumns - 1;
            bool topEdge = nodes.IndexOf(node) / collumns == 0;
            bool botEdge = nodes.IndexOf(node) / collumns == rows - 1;
            
            if (topEdge && leftEdge)
            {
                Debug.Log("Top Left Corner");
            }
            else if (topEdge && rightEdge)
            {
                Debug.Log("Top Right Corner");
            }
            else if (botEdge && leftEdge)
            {
                Debug.Log("Bottom Left Corner");
            }
            else if (botEdge && rightEdge)
            {
                Debug.Log("Bottom Right Corner");
            }
            else if (topEdge)
            {
                Debug.Log("Top Edge");
            }
            else if (botEdge)
            {
                Debug.Log("Bottom Edge");
            }
            else if (leftEdge)
            {
                Debug.Log("Left Edge");
            }
            else if (rightEdge)
            {
                Debug.Log("Right Edge");
            }
            else
            {
                Debug.Log("Middle");
            }
        }
    }
}

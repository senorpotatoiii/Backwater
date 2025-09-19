using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Class <c>NodeSpawner</c> creates a grid of <c>Node</c> objects and
/// sets their connections automatically.
/// </para>
/// <see cref="Node.cs"/>
/// </summary>
public class NodeSpawner : MonoBehaviour
{
    [SerializeField] Node prefab;
    [SerializeField] float yUnits = 3;
    [SerializeField] float xUnits = 3;
    [SerializeField] float nodesPerUnit = 1f;
    List<Node> nodes = new List<Node>();
    int rows;
    int collumns;
    
    void Awake()
    {
        rows = (int)(yUnits * nodesPerUnit);
        collumns = (int)(xUnits * nodesPerUnit);
        CreateNodes();
    }
    
    /// <summary>
    /// Creates a grid of <c>Node</c> objects, expanding right and down.
    /// <para>Designed without obstacles or terrain in mind.</para>
    /// </summary>
    void CreateNodes()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {
                Vector3 newPosition = new Vector3(transform.position.x + (j / nodesPerUnit),
                                                    transform.position.y - (i / nodesPerUnit));
                Node newNode = Instantiate(prefab, newPosition, Quaternion.identity, transform);
                nodes.Add(newNode);

                // Creates connections with nodes behind currently created node if able.
                int index = i * collumns + j;
                if (index % collumns != 0)
                {
                    newNode.connections.Add(nodes[index - 1]);
                    nodes[index - 1].connections.Add(newNode);
                }
                if (index / collumns != 0)
                {
                    newNode.connections.Add(nodes[index - collumns]);
                    nodes[index - collumns].connections.Add(newNode);
                    if (index % collumns != 0)
                    {
                        newNode.connections.Add(nodes[index - collumns - 1]);
                        nodes[index - collumns - 1].connections.Add(newNode);
                    }
                    if (index % collumns != collumns - 1)
                    {
                        newNode.connections.Add(nodes[index - collumns + 1]);
                        nodes[index - collumns + 1].connections.Add(newNode);
                    }
                }
            }
        }
    }
}

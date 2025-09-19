using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines the shortest path between two <c>Nodes</c>.
/// <para>
/// Evaluates each open <c>Node</c>'s <c>fScore</c>, choosing the <c>Node</c> with
/// the lowest score to continue exploring <c>Nodes</c>. This process is repeated
/// until the end <c>Node</c> is found, and therefore the shortest path.
/// </para>
/// <see cref="Node.cs"/>
/// </summary>
public class AStarManager : MonoBehaviour
{
    [HideInInspector] public static AStarManager instance;

    void Awake()
    {
        instance = this;
    }
    
    /// <summary>
    /// Finds the shortest path through a list of <c>Nodes</c> in the game world.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns>A list of <c>Nodes</c> ordered to be the shortest path from a starting <c>Node</c> to an end <c>Node</c>.</returns>
    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();
        
        foreach (Node n in FindObjectsOfType<Node>())
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            // Index of the node in the set with the lowest fScore.
            int lowestF = 0;
            
            // Finds the open node with the lowest fScore.
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FScore < openSet[lowestF].FScore)
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);
            
            if (currentNode == end)
            {
                List<Node> path = new List<Node>();
                path.Add(end);
                
                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }
                path.Reverse();
                return path;
            }
            
            foreach (Node connectedNode in currentNode.connections)
            {
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);
                
                if (heldGScore < connectedNode.gScore)
                {
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);
                    
                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }
        
        return null;
    }
}

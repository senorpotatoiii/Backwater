using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nodes to be used in <c>AStarManager</c> class.
/// <para>
/// Each node has:
/// <list type="bullet">
/// <item>
/// <term>gScore</term>
/// <description>Distance from starting point.</description>
/// </item>
/// <item>
/// <term>hScore</term>
/// <description>Distance from end point.</description>
/// </item>
/// <item>
/// <term>fScore</term>
/// <description>Sum of <c>gScore</c> and <c>hScore</c>.</description>
/// </item>
/// </list>
/// </para>
/// <see cref="AStarManager.cs"/>
/// </summary>
public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;
    
    /// <value>Distance from starting point.</value>
    public float gScore;
    /// <value>Distance from end point.</value>
    public float hScore;
    /// <value>Sum of gScore and hScore.</value>
    public float FScore { get => gScore + hScore; }

    // Used to debug the connections of nodes.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        if (connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connections[i].transform.position);
            }
        }
    }
}

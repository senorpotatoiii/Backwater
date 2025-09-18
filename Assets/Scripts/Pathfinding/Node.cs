using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nodes to be used in <c>AStarManager</c> class. Each node has:
/// <list type="bullet">
/// <item>Test</item>
/// </list>
/// </summary>
public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;
    public float gScore;
    public float hScore;
    public float fScore { get => gScore + hScore; }
}

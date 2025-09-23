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
    [SerializeField] private Node _prefab;
    [SerializeField] private float _yUnits = 3;
    [SerializeField] private float _xUnits = 3;
    [SerializeField] private float _nodesPerUnit = 1f;
    private List<Node> _nodes = new();
    private int _rows;
    private int _collumns;

    [SerializeField] private bool _debuggingMode = false;
    
    private void Awake()
    {
        _rows = (int)(_yUnits * _nodesPerUnit);
        _collumns = (int)(_xUnits * _nodesPerUnit);
        CreateNodes();
    }
    
    /// <summary>
    /// Creates a grid of <c>Node</c> objects, expanding right and down.
    /// <para>Designed without obstacles or terrain in mind.</para>
    /// </summary>
    private void CreateNodes()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _collumns; j++)
            {
                Vector3 newPosition = new Vector3(transform.position.x + (j / _nodesPerUnit),
                                                    transform.position.y - (i / _nodesPerUnit));
                Node newNode = Instantiate(_prefab, newPosition, Quaternion.identity, transform);
                _nodes.Add(newNode);

                /*
                 * As each node spawns, it creates up to 4 connections with the nodes around it
                 * Left, up, left-up, right-up
                 * Each connection is checked individually to account for any gaps in the graph.
                 */
                int index = i * _collumns + j;
                if (index % _collumns != 0)
                {
                    newNode.Connections.Add(_nodes[index - 1]);
                    _nodes[index - 1].Connections.Add(newNode);
                }
                if (index / _collumns != 0)
                {
                    newNode.Connections.Add(_nodes[index - _collumns]);
                    _nodes[index - _collumns].Connections.Add(newNode);
                    if (index % _collumns != 0)
                    {
                        newNode.Connections.Add(_nodes[index - _collumns - 1]);
                        _nodes[index - _collumns - 1].Connections.Add(newNode);
                    }
                    if (index % _collumns != _collumns - 1)
                    {
                        newNode.Connections.Add(_nodes[index - _collumns + 1]);
                        _nodes[index - _collumns + 1].Connections.Add(newNode);
                    }
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!_debuggingMode) { return; }
        Gizmos.color = Color.blue;

        foreach (Node n in _nodes)
        {
            if (n.Connections.Count > 0)
            {
                for (int i = 0; i < n.Connections.Count; i++)
                {
                    Gizmos.DrawLine(n.transform.position, n.Connections[i].transform.position);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Changes the camera bounds from one area to another within
/// the same scene.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class MapUpdate : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _mapBoundry1;
    [SerializeField] private PolygonCollider2D _mapBoundry2;
    private PolygonCollider2D _currentBoundry;
    private CinemachineConfiner _confiner;

    private void Awake()
    {
        _confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeCameraBounds();
            CenterPlayerPosition(collision.gameObject);
        }
    }

    /*
     * Checks to make sure that the player has moved to the
     * correct bounds, and changes the camera bounds if they
     * haven't.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_currentBoundry.bounds.Contains(collision.gameObject.transform.position))
            {
                ChangeCameraBounds();
            }
        }
    }

    /// <summary>
    /// Changes the camera bounds to the bound not currently active.
    /// </summary>
    private void ChangeCameraBounds()
    {
        if (!_mapBoundry1 || !_mapBoundry2) { return; }
        if (_confiner.m_BoundingShape2D == _mapBoundry1)
        {
            _confiner.m_BoundingShape2D = _currentBoundry = _mapBoundry2;
        }
        else if (_confiner.m_BoundingShape2D == _mapBoundry2)
        {
            _confiner.m_BoundingShape2D = _currentBoundry = _mapBoundry1;
        }
    }

    private void CenterPlayerPosition(GameObject player)
    {
        player.transform.position = transform.position;
    }
}

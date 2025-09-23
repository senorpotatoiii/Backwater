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
            ChangeCameraBounds(_mapBoundry1, _mapBoundry2);
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
            Debug.Log(_currentBoundry.gameObject.name);
            if (false)
            {
                ChangeCameraBounds(_mapBoundry1, _mapBoundry2);
            }
        }
    }

    private void ChangeCameraBounds(PolygonCollider2D collider1, PolygonCollider2D collider2)
    {
        if (!collider1 || !collider2) { return; }
        if (_confiner.m_BoundingShape2D == collider1)
        {
            _confiner.m_BoundingShape2D = _currentBoundry = collider2;
        }
        else if (_confiner.m_BoundingShape2D == collider2)
        {
            _confiner.m_BoundingShape2D = _currentBoundry = collider1;
        }
    }

    private void CenterPlayerPosition(GameObject player)
    {
        player.transform.position = transform.position;
    }
}

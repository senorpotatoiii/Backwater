using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void TestMove()
    {
        Debug.Log("Test");
        // Call MoveTo method.
    }
    
    // In progress
    void MoveTo(Vector3 targetPosition)
    {
        Vector2 deltaPosition = targetPosition - transform.position;
    }
}

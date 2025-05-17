using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    Vector3 rawInput = new Vector2();
    Vector2 newPosition;

    void Update()
    {
        if (rawInput != Vector3.zero)
        {
            Movement();
        }
    }
    
    void Movement()
    {
        newPosition =  (rawInput * movementSpeed * Time.deltaTime) + transform.position;
        
        // Check if the player is somewhere they are not supposed to be.
        //
        //
        //
        
        transform.position = newPosition;
    }

    // Is called automatically through the Player Input system.
    void OnMove(InputValue value) 
    {
        // Returns value as a 2D Vector and assigns it to rawInput.
        rawInput = value.Get<Vector2>();
    }
}

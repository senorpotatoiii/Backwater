using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    Rigidbody2D rb;
    
    public void SetMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Is called automatically through the Player Input system when a
    // button is pressed and when it is released.
    void OnMove(InputValue value) 
    {
        /*
         * Sets the velocity of the player's RigidBody2D component to the direction the player
         * is going. This is stored in value and normalized so that they are not faster when moving
         * along diagonals.
         */
        rb.velocity = value.Get<Vector2>().normalized * movementSpeed;
    }
}

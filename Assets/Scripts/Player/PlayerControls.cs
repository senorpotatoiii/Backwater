using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    FieldOfView fieldOfView;
    Rigidbody2D rb;
    Vector2 rawInput;
    
    public void SetMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fieldOfView = FindObjectOfType<FieldOfView>();
    }
    
    void Update()
    {
        rb.velocity = rawInput * movementSpeed;
        UpdateFieldOfView();
    }

    void UpdateFieldOfView()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        fieldOfView.SetAimDirection((targetPosition - transform.position).normalized);
        fieldOfView.SetOrigin(transform.position);
    }

    /*
     * Is called automatically through the Player Input system when a
     * button is pressed and when it is released.
     */
    public void Move(InputAction.CallbackContext context) 
    {
        /*
         * Sets the velocity of the player's RigidBody2D component to the direction the player
         * is going. This is stored in value and normalized so that they are not faster when moving
         * along diagonals.
         */
        rawInput = context.ReadValue<Vector2>().normalized;
    }
}

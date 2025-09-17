using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] GameObject testNPC;
    [SerializeField] GameObject testObject;
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
        Move();
        UpdateFieldOfView();
    }

    void Move()
    {
        rb.velocity = rawInput * movementSpeed;
    }

    void UpdateFieldOfView()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        fieldOfView.SetAimDirection((targetPosition - transform.position).normalized);
        fieldOfView.SetOrigin(transform.position);
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
        rawInput = value.Get<Vector2>().normalized;
    }
    
    void OnFlashlight()
    {
        fieldOfView.SetFlashlight(!fieldOfView.GetFlashlight());
        fieldOfView.GetComponent<MeshRenderer>().enabled = fieldOfView.GetFlashlight();
    }
    
    void OnInteract()
    {
        if (testObject != null)
        testObject.GetComponent<Interactibility>().Interact();
    }
    
    void OnTest()
    {
        if (testObject != null)
        testNPC.GetComponent<NPCMovement>().TestMove();
    }
}

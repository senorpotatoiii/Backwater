using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contains the player movement and any specific controls
/// to do with the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1f;
    private FieldOfView _fieldOfView;
    private Rigidbody2D _rb;
    private Vector2 _rawInput;
    
    public float MovementSpeed { set => _movementSpeed = value; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _fieldOfView = FindObjectOfType<FieldOfView>();
    }
    
    private void Update()
    {
        _rb.velocity = _rawInput * _movementSpeed;
        UpdateFieldOfView();
    }

    /// <summary>
    /// <para>
    /// Sets the flightlight to be on the player pointed at the mouse.
    /// </para>
    /// <see cref="FieldOfView.cs"/>
    /// </summary>
    private void UpdateFieldOfView()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _fieldOfView.SetAimDirection((targetPosition - transform.position).normalized);
        _fieldOfView.SetOrigin(transform.position);
    }

    /// <summary>
    /// Is called automatically through the Player Input system when a
    /// button is pressed and when it is released.
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context) 
    {
        /*
         * Sets the velocity of the player's RigidBody2D component to the
         * direction the player is going. This is stored in value and normalized
         * so that they are not faster when moving along diagonals.
         */
        _rawInput = context.ReadValue<Vector2>().normalized;
    }
}

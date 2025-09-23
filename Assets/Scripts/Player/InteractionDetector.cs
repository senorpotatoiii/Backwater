using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Detects if an interactable object is in range of the player.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
        }
    }
}

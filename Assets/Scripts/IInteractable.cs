public interface IInteractable
{
    /// <summary>
    /// Executes whatever behavior happens when this object is
    /// interacted with.
    /// </summary>
    public void Interact();
    
    /// <summary>
    /// Checks if the object can be interacted with.
    /// </summary>
    /// <returns>
    /// True if the object can currently be interacted with, false if it cannot.
    /// </returns>
    public bool CanInteract();
}

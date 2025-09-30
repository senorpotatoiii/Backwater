using UnityEngine;

/// <summary>
/// <para>
/// Controller for all friendly AI.
/// </para>
/// <see cref="AI.cs"/>, <see cref="IInteractable.cs"/>
/// </summary>
public class NPCBehavior : AI, IInteractable
{
    enum State { Idle, Pathing }

    [Header("NPC")]
    [SerializeField] private State _state;
    [SerializeField] private Conversation _dialogueData;
    private bool _dialogueActive = false;
    
    private new void Awake()
    {
        base.Awake();
    }
    
    private new void Start()
    {
        base.Start();
        DialogueManager.s_Instance.DialogueFinished += FinishDialogue;
    }
    
    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Pathing:
                CreatePath();
                break;
        }
    }

    /// <summary>
    /// <para>
    /// Passes <c>Dialogue Data</c> into the <c>Dialogue Manager</c>.
    /// If there is no dialogue currently active, it opens the dialogue UI and
    /// prints the first line of dialogue. Successive calls print the next line
    /// until it reaches the last line.
    /// </para>
    /// <see cref="DialogueManager.cs"/>
    /// </summary>
    public void Interact()
    {
        if (_dialogueActive)
        {
            DialogueManager.s_Instance.NextLine();
        }
        else
        {
            DialogueManager.s_Instance.StartDialogue(_dialogueData);
            _dialogueActive = true;
        }
    }

    private void FinishDialogue()
    {
        _dialogueActive = false;
    }

    public bool CanInteract()
    {
        return !_dialogueActive;
    }
}

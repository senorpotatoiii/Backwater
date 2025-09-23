using UnityEngine;

/// <summary>
/// Controller for all friendly AI.
/// <para></para>
/// <see cref="AI.cs"/>, <see cref="IInteractable.cs"/>
/// </summary>
public class NPCBehavior : AI, IInteractable
{
    enum State { Idle, Pathing }

    [Header("NPC")]
    [SerializeField] private State _state;
    [SerializeField] private Dialogue _dialogueData;
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
    /// Passes <c>Dialogue Data</c> into the <c>Dialogue Manager</c>.
    /// <para>
    /// If there is no dialogue currently active, it opens the dialogue UI and
    /// prints the first line of dialogue. Successive calls print the next line
    /// until it reaches the last line.
    /// </para>
    /// <see cref="DialogueManager.cs"/>
    /// </summary>
    public void Interact()
    {
        if (!_dialogueData) return;
        
        if (_dialogueActive)
        {
            DialogueManager.s_Instance.NextLine(_dialogueData);
        }
        else
        {
            _dialogueActive = true;
            DialogueManager.s_Instance.StartDialogue(_dialogueData);
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

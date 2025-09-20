using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] State state;
    [SerializeField] Dialogue dialogueData;
    bool dialogueActive = false;
    
    new void Awake()
    {
        base.Awake();
    }
    
    new void Start()
    {
        base.Start();
        DialogueManager.instance.dialogueFinished += FinishDialogue;
    }
    
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Pathing:
                CreatePath();
                break;
        }
    }

    public void Interact()
    {
        if (!dialogueData) return;
        
        if (dialogueActive)
        {
            DialogueManager.instance.NextLine(dialogueData);
        }
        else
        {
            dialogueActive = true;
            DialogueManager.instance.StartDialogue(dialogueData);
        }
    }
    
    void FinishDialogue() { dialogueActive = false; }

    public bool CanInteract()
    {
        return !dialogueActive;
    }
}

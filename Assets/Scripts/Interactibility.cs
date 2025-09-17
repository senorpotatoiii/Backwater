using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactibility : MonoBehaviour
{
    enum InteractType { Dialogue, Event }

    [Header("Basic")]
    [SerializeField] InteractType interactType;
    DialogueExecutor dialogueManager;
    GameObject interactIcon;

    [Header("Dialogue")]
    [SerializeField] List<DialogueLines> dialogueLines = new List<DialogueLines>();
    
    bool canInteract;
    
    public bool CanInteract() { return canInteract; }

    void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueExecutor>();
    }

    void Start()
    {
        interactIcon = gameObject.transform.GetChild(0).gameObject;
        interactIcon.SetActive(false);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            interactIcon.SetActive(canInteract);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            interactIcon.SetActive(canInteract);
        }
    }
    
    public void Interact()
    {
        if (!canInteract) { return; }
        
        switch (interactType)
        {
            case InteractType.Dialogue:
                dialogueManager.DisplayLines(dialogueLines);
                break;
        }
    }
}

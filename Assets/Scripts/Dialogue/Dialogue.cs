using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName="NewDialogue", menuName="Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] bool isNPC;
    [SerializeField] string npcName;
    [SerializeField] Sprite portrait;
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] string[] dialogueLines;
    
    public bool IsNPC { get => isNPC; }
    public string NPCName { get => npcName; }
    public Sprite Portrait { get => portrait; }
    public float TypingSpeed{ get => typingSpeed; set => typingSpeed = value; }
    public string[] DialogueLines { get => dialogueLines; }
}

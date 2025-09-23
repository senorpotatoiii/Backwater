using UnityEngine;

/// <summary>
/// Stores all relevant data for dialogue scenes.
/// </summary>
[CreateAssetMenu(fileName="NewDialogue", menuName="Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private bool _isNPC;
    [SerializeField] private string _npcName;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private string[] _dialogueLines;
    
    public bool IsNPC { get => _isNPC; }
    public string NPCName { get => _npcName; }
    public Sprite Portrait { get => _portrait; }
    public float TypingSpeed{ get => _typingSpeed; set => _typingSpeed = value; }
    public string[] DialogueLines { get => _dialogueLines; }
}

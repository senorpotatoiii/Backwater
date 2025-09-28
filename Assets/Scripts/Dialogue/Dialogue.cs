using UnityEngine;

/// <summary>
/// Stores all relevant data for dialogue scenes.
/// </summary>
[CreateAssetMenu(fileName="NewDialogue", menuName="Dialogue")]
public class Dialogue : ScriptableObject
{
    enum Characters { Object, Detective, Daughter, OldMan }

    [SerializeField] private Characters character;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private string[] _dialogueLines;
    [SerializeField] private Dialogue _nextDialogue;
    
    public bool IsNPC { get => character != Characters.Object; }
    public string NPCName
    {
        get
        {
            switch (character)
            {
                case Characters.Detective:
                    return "Detective";
                case Characters.Daughter:
                    return "Daughter";
                case Characters.OldMan:
                    return "Old Man";
                default:
                    return "";
            }
        }
    }
    public Sprite Portrait
    {
        get
        {
            switch (character)
            {
                case Characters.Detective:
                    return SpriteAssets.s_Instance.Sprites["Detective_Portrait"];
                case Characters.Daughter:
                    return SpriteAssets.s_Instance.Sprites["Daughter_Portrait"];
                case Characters.OldMan:
                    return null;
                default:
                    return null;
            }
        }
    }
    public float TypingSpeed { get => _typingSpeed; set => _typingSpeed = value; }
    public string[] DialogueLines { get => _dialogueLines; }
    public Dialogue NextDialogue { get => _nextDialogue; }
}

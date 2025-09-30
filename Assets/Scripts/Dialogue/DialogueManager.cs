using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains all logic for executing dialogue scenes.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_Instance { get; private set; }

    [SerializeField] private GameObject _dialoguePannel;
    [SerializeField] private Image _portrait;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Transform _choiceContainer;
    [SerializeField] private GameObject _choiceButtonPrefab;
    private Conversation _dialogue;
    private int _dialogueIndex;
    private int _lineIndex;
    private bool _typing = false;

    /// <summary>
    /// Called when the current dialogue finishes its last line.
    /// </summary>
    public Action DialogueFinished;

    private void Awake()
    {
        if (!s_Instance) s_Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Sets up the dialogue scene.
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Conversation dialogue)
    {
        _dialogue = dialogue;
        _dialogueIndex = 0;
        _lineIndex = 0;
        if (_dialogue.Dialogues.Length == 0)
        {
            EndDialogue();
            return;
        }
        _dialoguePannel.SetActive(true);
        SetupSpeaker(_dialogue.Dialogues[_dialogueIndex]);
        StartCoroutine(TypeLine());
    }
    
    /// <summary>
    /// Sets the speaker portrait and name.
    /// </summary>
    /// <param name="speakerData"></param>
    private void SetupSpeaker(Dialogue speakerData)
    {
        if (speakerData.IsNPC)
        {
            _portrait.sprite = speakerData.Portrait;
            _name.SetText(speakerData.NPCName);
        }
        else
        {
            _portrait.sprite = null;
            _name.SetText("");
        }
    }
    
    /// <summary>
    /// <para>
    /// If a line is currently being typed, completes the line. Otherwise, checks the current speaker
    /// and line. If it's the last speaker and line, ends the dialogue. If not, switches to the next speaker
    /// if applicable and prints the next line.
    /// </para>
    /// <see cref="EndDialogue"/>
    /// </summary>
    public void NextLine()
    {
        if (_typing)
        {
            StopAllCoroutines();
            _dialogueText.SetText(_dialogue.Dialogues[_dialogueIndex].DialogueLines[_lineIndex]);
            _typing = false;
        }
        else if (++_lineIndex < _dialogue.Dialogues[_dialogueIndex].DialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else if (++_dialogueIndex < _dialogue.Dialogues.Length)
        {
            _lineIndex = 0;
            SetupSpeaker(_dialogue.Dialogues[_dialogueIndex]);
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary>
    /// Sets the current line to invisible. Then iterates through the characters, making them
    /// visible again.
    /// </summary>
    private IEnumerator TypeLine()
    {
        _typing = true;
        string line = _dialogue.Dialogues[_dialogueIndex].DialogueLines[_lineIndex];
        string charsRevealed;
        string charsHidden;
        for (int i = 0; i < line.Length; i++)
        {
            while (line[i] == ' ') i++;

            charsRevealed = line.Substring(0, ++i);
            charsHidden = line.Substring(i--, line.Length - charsRevealed.Length);
            _dialogueText.SetText(charsRevealed + "<color=#00000000>" + charsHidden + "</color>");

            yield return new WaitForSeconds(_dialogue.Dialogues[_dialogueIndex].TypingSpeed);
        }
        _typing = false;
    }

    /// <summary>
    /// Deactivates the dialogue UI pannel and calls the
    /// <c>DialogueFinished</c> Action.
    /// </summary>
    public void EndDialogue()
    {
        StopAllCoroutines();
        DialogueFinished?.Invoke();
        _dialoguePannel.SetActive(false);
    }
}

using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    private bool _choiceActive = false;

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
        DisplayCurrentLine();
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
    /// <see cref="TypeLine"/>
    /// </summary>
    public void NextLine()
    {
        if (_choiceActive) return;
        if (_typing)
        {
            StopAllCoroutines();
            _dialogueText.SetText(_dialogue.Dialogues[_dialogueIndex].DialogueLines[_lineIndex]);
            _typing = false;
            CheckChoice();
            return;
        }

        if (++_lineIndex < _dialogue.Dialogues[_dialogueIndex].DialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else if (_dialogue.Dialogues[_dialogueIndex].Choices.Length == 0)
        {
            _lineIndex = 0;
            _dialogueIndex = _dialogue.Dialogues[_dialogueIndex].NextIndex;
            if (_dialogueIndex == -1)
            {
                EndDialogue();
                return;
            }
            SetupSpeaker(_dialogue.Dialogues[_dialogueIndex]);
            DisplayCurrentLine();
        }
    }
    
    /// <summary>
    /// <para>
    /// Stops all coroutines and prints the next line of dialogue.
    /// </para>
    /// <see cref="TypeLine"/>
    /// </summary>
    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
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
        CheckChoice();
    }
    
    /// <summary>
    /// If the current line is the last in a dialogue that has a player
    /// choice, create choice buttons and set <c>choiceActive</c> to true.
    /// </summary>
    private void CheckChoice()
    {
        if (_lineIndex == _dialogue.Dialogues[_dialogueIndex].DialogueLines.Length - 1
            && _dialogue.Dialogues[_dialogueIndex].Choices.Length > 0)
        {
            _choiceActive = true;
            foreach (Choices choice in _dialogue.Dialogues[_dialogueIndex].Choices)
            {
                CreateChoiceButton(choice.Choice, () => SetChoice(choice.IndexTo));
            }
        }
    }

    /// <summary>
    /// <para>
    /// Sets the <c>dialogueIndex</c> to the one specified by the dialogue
    /// choice, then clears the buttons and types the next line.
    /// </para>
    /// <see cref="TypeLine"/>
    /// </summary>
    /// <param name="index"></param>
    public void SetChoice(int index)
    {
        _dialogueIndex = index;
        foreach (Transform child in _choiceContainer)
        {
            Destroy(child.gameObject);
        }
        DisplayCurrentLine();
        _choiceActive = false;
    }

    /// <summary>
    /// Creates a button as a child of the dialogue choice pannel.
    /// </summary>
    /// <param name="choiceText"></param>
    /// <param name="onClick"></param>
    /// <returns></returns>
    private GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(_choiceButtonPrefab, _choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        return choiceButton;
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

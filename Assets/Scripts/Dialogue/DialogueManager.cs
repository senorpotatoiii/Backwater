using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public static DialogueManager s_Instance;
    
    [SerializeField] private GameObject _dialoguePannel;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private TMP_Text _nameText, _dialogueText;
    private bool _isTyping;
    private int _dialogueIndex;
    
    /// <summary>
    /// Called when the current dialogue finishes its last line.
    /// </summary>
    public Action DialogueFinished;
    
    private void Awake()
    {
        s_Instance = this;
    }

    /// <summary>
    /// <para>
    /// Initializes all necessary values and text fields, then starts
    /// the typing coroutine.
    /// </para>
    /// <see cref="TypeLine"/>
    /// </summary>
    /// <param name="data"></param>
    public void StartDialogue(Dialogue data)
    {
        if (data.IsNPC)
        {
            _nameText.SetText(data.NPCName);
            _portraitImage.gameObject.SetActive(true);
            _portraitImage.sprite = data.Portrait;
        }
        else
        {
            _nameText.SetText("");
            _portraitImage.gameObject.SetActive(false);
        }
        _dialogueIndex = 0;
        _dialoguePannel.SetActive(true);
        StartCoroutine(TypeLine(data));
    }
    
    /// <summary>
    /// <para>
    /// If a line is currently being printed, skips to the end of the line.
    /// Otherwise, starts printing the next line if one is available and
    /// ends the dialogue if there isn't.
    /// </para>
    /// <see cref="TypeLine"/>, <see cref="EndDialogue"/>
    /// </summary>
    /// <param name="data"></param>
    public void NextLine(Dialogue data)
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            _dialogueText.SetText(data.DialogueLines[_dialogueIndex]);
            _isTyping = false;
        }
        else if (++_dialogueIndex < data.DialogueLines.Length)
        {
            StartCoroutine(TypeLine(data));
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary>
    /// Sets the alpha of the current line to 0, turning the text invisible.
    /// Then iterates through the characters in the line, turning all
    /// characters revealed to far visible again.
    /// </summary>
    /// <param name="data"></param>
    private IEnumerator TypeLine(Dialogue data)
    {
        _isTyping = true;
        string currentLine = data.DialogueLines[_dialogueIndex];
        _dialogueText.SetText("<color=#00000000>" + currentLine + "</color>");
        string printedCharacters;
        string hiddenCharacters;

        for (int i = 0; i < currentLine.Length; i++)
        {
            while (currentLine[i] == ' ') i++;
            printedCharacters = currentLine.Substring(0, ++i);
            hiddenCharacters = currentLine.Substring(i--, currentLine.Length - printedCharacters.Length);

            _dialogueText.SetText(printedCharacters + "<color=#00000000>" + hiddenCharacters + "</color>");
            yield return new WaitForSeconds(data.TypingSpeed);
        }

        _isTyping = false;
    }
    
    /// <summary>
    /// <para>
    /// Stops all typing, calls the <c>Dialogue Finished</c> event,
    /// and diables the dialogue UI.
    /// </para>
    /// <see cref="DialogueFinished"/>
    /// </summary>
    public void EndDialogue()
    {
        StopAllCoroutines();
        DialogueFinished?.Invoke();
        _dialogueText.SetText("");
        _dialoguePannel.SetActive(false);
    }
}

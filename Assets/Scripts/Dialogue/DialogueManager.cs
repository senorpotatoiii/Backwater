using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public static DialogueManager instance;
    [SerializeField] GameObject dialoguePannel;
    [SerializeField] Image portraitImage;
    [SerializeField] TMP_Text nameText, dialogueText;
    bool isTyping;
    int dialogueIndex;
    public Action dialogueFinished;
    
    void Awake()
    {
        instance = this;
    }

    public void StartDialogue(Dialogue data)
    {
        if (data.IsNPC)
        {
            nameText.SetText(data.NPCName);
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = data.Portrait;
        }
        else
        {
            nameText.SetText("");
            portraitImage.gameObject.SetActive(false);
        }
        dialogueIndex = 0;
        dialoguePannel.SetActive(true);
        StartCoroutine(TypeLine(data));
    }
    
    public void NextLine(Dialogue data)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(data.DialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < data.DialogueLines.Length)
        {
            StartCoroutine(TypeLine(data));
        }
        else
        {
            EndDialogue();
        }
    }
    
    IEnumerator TypeLine(Dialogue data)
    {
        isTyping = true;
        string currentLine = data.DialogueLines[dialogueIndex];
        dialogueText.SetText("<color=#00000000>" + currentLine + "</color>");
        string printedCharacters;
        string hiddenCharacters;

        int i = 0;
        while (i < currentLine.Length)
        {
            while (currentLine[i] == ' ')
            {
                i++;
            }
            printedCharacters = currentLine.Substring(0, ++i);
            hiddenCharacters = currentLine.Substring(i, currentLine.Length - printedCharacters.Length);

            dialogueText.SetText(printedCharacters + "<color=#00000000>" + hiddenCharacters + "</color>");
            yield return new WaitForSeconds(data.TypingSpeed);
        }

        isTyping = false;
    }
    
    public void EndDialogue()
    {
        StopAllCoroutines();
        dialogueFinished?.Invoke();
        dialogueText.SetText("");
        dialoguePannel.SetActive(false);
    }
}

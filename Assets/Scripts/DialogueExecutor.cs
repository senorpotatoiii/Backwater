using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueExecutor : MonoBehaviour
{
    public void DisplayLines(List<DialogueLines> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Debug.Log(lines[i].GetDialogue());
        }
    }
}

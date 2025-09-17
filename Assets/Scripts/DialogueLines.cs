using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLines
{
    [SerializeField] string line;
    
    public string GetDialogue() { return line; }
}

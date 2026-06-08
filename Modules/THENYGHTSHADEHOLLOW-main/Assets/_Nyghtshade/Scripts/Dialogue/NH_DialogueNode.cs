using System;
using System.Collections.Generic;

[Serializable]
public class NH_DialogueNode
{
    public string nodeId;
    public string speakerName;
    [UnityEngine.TextArea] public string text;
    public List<NH_DialogueChoice> choices = new List<NH_DialogueChoice>();
    public List<NH_DialogueCondition> entryConditions = new List<NH_DialogueCondition>();
    public string setFlagOnEnter;
    public string questIdToAdvance;
    public bool isEndNode;
}

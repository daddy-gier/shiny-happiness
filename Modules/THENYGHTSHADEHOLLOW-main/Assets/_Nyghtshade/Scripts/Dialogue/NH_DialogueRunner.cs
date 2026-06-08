using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_DialogueRunner : MonoBehaviour
{
    public static NH_DialogueRunner Instance { get; private set; }

    private List<NH_DialogueNode> _currentConversation;
    private NH_DialogueNode _currentNode;
    private NH_PlayerStats _currentPlayer;

    public event Action<NH_DialogueNode> OnNodeDisplayed;
    public event Action<List<NH_DialogueChoice>> OnChoicesPresented;
    public event Action OnDialogueEnded;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartConversation(List<NH_DialogueNode> nodes, NH_PlayerStats player)
    {
        if (nodes == null || nodes.Count == 0) return;
        _currentConversation = nodes;
        _currentPlayer = player;
        DisplayNode(nodes[0]);
    }

    void DisplayNode(NH_DialogueNode node)
    {
        _currentNode = node;

        if (!string.IsNullOrEmpty(node.setFlagOnEnter))
            NH_StoryFlagManager.Instance.SetFlag(node.setFlagOnEnter);

        if (!string.IsNullOrEmpty(node.questIdToAdvance))
            NH_QuestManager.Instance?.AdvanceQuest(node.questIdToAdvance);

        OnNodeDisplayed?.Invoke(node);

        if (node.isEndNode || node.choices.Count == 0)
        {
            OnDialogueEnded?.Invoke();
            return;
        }

        var validChoices = node.choices.FindAll(c =>
            string.IsNullOrEmpty(c.requiredFlag) || NH_StoryFlagManager.Instance.HasFlag(c.requiredFlag));
        OnChoicesPresented?.Invoke(validChoices);
    }

    public void SelectChoice(NH_DialogueChoice choice)
    {
        if (!string.IsNullOrEmpty(choice.setFlagOnChoose))
            NH_StoryFlagManager.Instance.SetFlag(choice.setFlagOnChoose);

        if (!string.IsNullOrEmpty(choice.requiredFactionId) && Mathf.Abs(choice.factionReputationDelta) > 0.001f)
            NH_FactionManager.Instance?.ModifyRespect(choice.requiredFactionId, choice.factionReputationDelta);

        if (choice.endsDialogue || string.IsNullOrEmpty(choice.nextNodeId))
        {
            OnDialogueEnded?.Invoke();
            return;
        }

        var nextNode = _currentConversation?.Find(n => n.nodeId == choice.nextNodeId);
        if (nextNode != null) DisplayNode(nextNode);
        else OnDialogueEnded?.Invoke();
    }
}

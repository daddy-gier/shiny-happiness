using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Nyghtshade/Quest Definition")]
public class NH_QuestDefinition : ScriptableObject
{
    [Header("Identity")]
    public string questId;
    public string displayName;
    [TextArea] public string description;
    public string category;

    [Header("Requirements")]
    public string requiredFlag;
    public string requiredFactionId;
    public float minFactionReputation;

    [Header("Objectives")]
    public List<NH_QuestObjective> objectives = new List<NH_QuestObjective>();

    [Header("Rewards")]
    public float reputationReward;
    public string rewardFactionId;
    public string setFlagOnComplete;
    public string unlockQuestId;
}

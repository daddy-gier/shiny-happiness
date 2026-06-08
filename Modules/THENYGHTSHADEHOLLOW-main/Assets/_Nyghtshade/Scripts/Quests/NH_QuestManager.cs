using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_QuestManager : MonoBehaviour
{
    public static NH_QuestManager Instance { get; private set; }

    [Header("All Quests")]
    public List<NH_QuestDefinition> allQuests = new List<NH_QuestDefinition>();

    private Dictionary<string, NH_QuestState> _questStates = new Dictionary<string, NH_QuestState>();

    public event Action<string, NH_QuestState> OnQuestStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var q in allQuests)
            _questStates[q.questId] = NH_QuestState.NotStarted;
    }

    public NH_QuestState GetState(string questId)
    {
        _questStates.TryGetValue(questId, out var s);
        return s;
    }

    public void SetState(string questId, NH_QuestState state)
    {
        _questStates[questId] = state;
        OnQuestStateChanged?.Invoke(questId, state);

        if (state == NH_QuestState.Completed)
        {
            var def = allQuests.Find(q => q.questId == questId);
            if (def != null)
            {
                if (!string.IsNullOrEmpty(def.setFlagOnComplete))
                    NH_StoryFlagManager.Instance.SetFlag(def.setFlagOnComplete);
                if (!string.IsNullOrEmpty(def.rewardFactionId) && def.reputationReward != 0f)
                    NH_FactionManager.Instance?.ModifyRespect(def.rewardFactionId, def.reputationReward);
            }
        }
    }

    public void AdvanceQuest(string questId)
    {
        var state = GetState(questId);
        if (state == NH_QuestState.NotStarted) SetState(questId, NH_QuestState.Accepted);
        else if (state == NH_QuestState.Accepted || state == NH_QuestState.InProgress)
            SetState(questId, NH_QuestState.InProgress);
    }

    public void CompleteQuest(string questId) => SetState(questId, NH_QuestState.Completed);
    public void FailQuest(string questId) => SetState(questId, NH_QuestState.Failed);
}

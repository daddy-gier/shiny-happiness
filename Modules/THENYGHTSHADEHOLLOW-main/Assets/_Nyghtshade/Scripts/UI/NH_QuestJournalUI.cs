using UnityEngine;
using TMPro;

public class NH_QuestJournalUI : MonoBehaviour
{
    public TMP_Text journalText;

    void OnEnable() => Refresh();

    void Refresh()
    {
        if (journalText == null || NH_QuestManager.Instance == null) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("=== QUEST JOURNAL ===\n");

        foreach (var quest in NH_QuestManager.Instance.allQuests)
        {
            var state = NH_QuestManager.Instance.GetState(quest.questId);
            if (state == NH_QuestState.Hidden || state == NH_QuestState.NotStarted) continue;
            sb.AppendLine($"[{state}] {quest.displayName}");
            sb.AppendLine($"  {quest.description}\n");
        }

        journalText.text = sb.ToString();
    }
}

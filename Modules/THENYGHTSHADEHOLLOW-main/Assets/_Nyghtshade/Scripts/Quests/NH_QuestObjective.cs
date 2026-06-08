using System;

[Serializable]
public class NH_QuestObjective
{
    public string objectiveId;
    public string description;
    public bool isCompleted;
    public bool isOptional;
    public string completionFlag;
    public string requiredItemId;
    public int requiredCount = 1;
    public int currentCount;

    public bool CheckCompletion()
    {
        if (!string.IsNullOrEmpty(completionFlag))
            return NH_StoryFlagManager.Instance.HasFlag(completionFlag);
        if (!string.IsNullOrEmpty(requiredItemId))
            return currentCount >= requiredCount;
        return isCompleted;
    }
}

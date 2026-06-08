using UnityEngine;

[System.Serializable]
public class NH_DialogueCondition
{
    public enum ConditionType { StoryFlag, HasItem, FactionReputation, ScheduleBlock, PlayerPath }

    public ConditionType conditionType;
    public string stringParam;
    public float floatParam;
    public bool invert;

    public bool Evaluate(NH_PlayerStats player)
    {
        bool result = conditionType switch
        {
            ConditionType.StoryFlag => NH_StoryFlagManager.Instance.HasFlag(stringParam),
            ConditionType.HasItem => player.GetComponent<NH_PlayerInventory>()?.HasItem(stringParam) ?? false,
            ConditionType.FactionReputation => NH_FactionManager.Instance?.GetStanding(stringParam).respect >= floatParam,
            ConditionType.ScheduleBlock => NH_PrisonScheduleManager.Instance?.CurrentBlockName == stringParam,
            _ => true
        };
        return invert ? !result : result;
    }
}

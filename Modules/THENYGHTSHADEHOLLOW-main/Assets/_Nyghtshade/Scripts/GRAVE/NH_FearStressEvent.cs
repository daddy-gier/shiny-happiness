using UnityEngine;

public class NH_FearStressEvent : MonoBehaviour
{
    public string eventId;
    [TextArea] public string description;
    public float stressAmount = 20f;
    public float fearAmount = 15f;
    public string setFlagOnTrigger;
    public bool oneShot = true;
    private bool _triggered;

    public void TriggerEvent(NH_PlayerStats player)
    {
        if (oneShot && _triggered) return;
        _triggered = true;

        player.AddStress(stressAmount);
        player.GetComponent<NH_SurvivalStatus>()?.ModifyFear(fearAmount);

        if (!string.IsNullOrEmpty(setFlagOnTrigger))
            NH_StoryFlagManager.Instance.SetFlag(setFlagOnTrigger);

        Debug.Log($"[GRAVE] Fear event: {eventId} - {description}");
    }
}

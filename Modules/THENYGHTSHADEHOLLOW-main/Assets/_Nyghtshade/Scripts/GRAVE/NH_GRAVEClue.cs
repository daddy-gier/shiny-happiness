using UnityEngine;

public class NH_GRAVEClue : NH_InteractableBase
{
    [Header("GRAVE Clue")]
    public string clueId;
    [TextArea] public string visionText;
    public bool isConcealed = true;
    public string revealFlag;
    public string connectionSource;

    public override string DisplayName => isConcealed ? "Strange Marking" : clueId;
    public override string InteractVerb => "Examine";

    public override void Interact(NH_PlayerStats player)
    {
        isConcealed = false;
        if (!string.IsNullOrEmpty(revealFlag))
            NH_StoryFlagManager.Instance.SetFlag(revealFlag);

        NH_GRAVEManager.Instance?.DiscoverClue(clueId);
        player.AddStress(15f);
        Debug.Log($"[GRAVE] Clue discovered: {clueId} - {visionText}");
    }
}

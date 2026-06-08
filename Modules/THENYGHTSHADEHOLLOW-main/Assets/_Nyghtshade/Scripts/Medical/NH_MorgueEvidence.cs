using UnityEngine;

public class NH_MorgueEvidence : NH_InteractableBase
{
    [Header("Morgue Evidence")]
    public NH_CustodyDeathCase deathCase;
    public string evidenceId;
    public bool isHidden = true;
    public string setFlagOnCollect;
    public bool triggerInvestigation;

    public override string DisplayName => isHidden ? "Suspicious Document" : (deathCase?.victimName + " File") ?? "Evidence";
    public override string InteractVerb => "Examine";

    public override void Interact(NH_PlayerStats player)
    {
        if (deathCase != null) deathCase.evidenceDiscovered = true;
        isHidden = false;

        var inv = player.GetComponent<NH_PlayerInventory>();

        if (!string.IsNullOrEmpty(setFlagOnCollect))
            NH_StoryFlagManager.Instance.SetFlag(setFlagOnCollect);

        if (triggerInvestigation && deathCase != null && !string.IsNullOrEmpty(deathCase.relatedQuestId))
            NH_QuestManager.Instance?.AdvanceQuest(deathCase.relatedQuestId);

        if (deathCase != null && deathCase.linksToGRAVE)
            NH_GRAVEManager.Instance?.RevealConnection("morgue_evidence");

        Debug.Log($"[NH] Morgue evidence examined: {evidenceId}");
    }
}

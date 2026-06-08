using System;

[Serializable]
public class NH_DialogueChoice
{
    public string choiceText;
    public string nextNodeId;
    public string requiredFlag;
    public string requiredItemId;
    public string requiredFactionId;
    public float minFactionReputation;
    public string setFlagOnChoose;
    public float factionReputationDelta;
    public string giveItemId;
    public bool endsDialogue;
}

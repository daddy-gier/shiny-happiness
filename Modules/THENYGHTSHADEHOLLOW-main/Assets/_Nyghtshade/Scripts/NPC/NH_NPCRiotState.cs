using UnityEngine;

public class NH_NPCRiotState : MonoBehaviour
{
    public NH_FactionRiotStance riotStance = NH_FactionRiotStance.Neutral;
    public bool isLeader;
    public bool hasBeenCaptured;
    public float riotAggressionLevel;

    private NH_NPCBrain _brain;

    void Awake() => _brain = GetComponent<NH_NPCBrain>();

    public void ApplyRiotStance(NH_FactionRiotStance stance)
    {
        riotStance = stance;
        if (_brain == null) return;

        switch (stance)
        {
            case NH_FactionRiotStance.JoinRiot:
                _brain.SetState(NH_NPCState.RiotActive);
                riotAggressionLevel = Random.Range(50f, 100f);
                break;
            case NH_FactionRiotStance.Avoid:
            case NH_FactionRiotStance.Neutral:
                _brain.SetState(NH_NPCState.Hiding);
                break;
            case NH_FactionRiotStance.DefendTerritory:
                _brain.SetState(NH_NPCState.GuardingArea);
                break;
            case NH_FactionRiotStance.Negotiate:
                _brain.SetState(NH_NPCState.Negotiating);
                break;
        }
    }
}

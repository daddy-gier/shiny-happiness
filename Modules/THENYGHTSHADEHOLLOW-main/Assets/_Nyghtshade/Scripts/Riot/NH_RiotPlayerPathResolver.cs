using UnityEngine;

public class NH_RiotPlayerPathResolver : MonoBehaviour
{
    public static NH_RiotPlayerPathResolver Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ResolvePlayerAction(NH_PlayerStats player, NH_RiotPhase phase, string choiceId)
    {
        switch (choiceId)
        {
            case "hide":
                player.AddStress(-5f);
                player.ReduceSuspicion(10f);
                break;
            case "exploit_escape_window":
                if (player.pathEscapeArtist)
                    NH_StoryFlagManager.Instance.SetFlag("riot_escape_window_used");
                break;
            case "capture_zone":
                if (player.pathYardKing)
                    NH_FactionManager.Instance?.ModifyRespect("HollowKings", 15f);
                break;
            case "access_evidence":
                if (player.pathInvestigator)
                    NH_StoryFlagManager.Instance.SetFlag("riot_evidence_accessed");
                break;
            case "rescue_npc":
                if (player.pathRedeemer)
                {
                    player.AddStress(10f);
                    NH_FactionManager.Instance?.ModifyTrust("ChapelFlock", 10f);
                }
                break;
            case "grave_breach":
                if (player.pathGRAVE)
                    NH_GRAVEManager.Instance?.RevealConnection("riot_breach");
                break;
        }
    }
}

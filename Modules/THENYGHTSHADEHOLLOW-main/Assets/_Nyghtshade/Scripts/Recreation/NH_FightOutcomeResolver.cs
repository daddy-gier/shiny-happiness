using UnityEngine;

public class NH_FightOutcomeResolver : MonoBehaviour
{
    public static NH_FightOutcomeResolver Instance { get; private set; }

    [Header("Fight Parameters")]
    public float playerSkillWeight = 0.6f;
    public float staminaWeight = 0.3f;
    public float luckWeight = 0.1f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public NH_FightResult ResolveFight(NH_PlayerStats player, float opponentDifficulty)
    {
        float playerScore = (player.Stamina / 100f) * staminaWeight +
                            (1f - (player.Stress / 100f)) * playerSkillWeight +
                            Random.value * luckWeight;

        float opponentScore = opponentDifficulty / 100f + Random.value * 0.2f;

        if (playerScore > opponentScore)
        {
            player.AddStress(10f);
            NH_FactionManager.Instance?.ModifyRespect("HollowKings", 5f);
            return NH_FightResult.Victory;
        }
        else
        {
            player.TakeDamage(20f);
            player.AddStress(25f);
            return NH_FightResult.Defeat;
        }
    }
}

public enum NH_FightResult { Victory, Defeat, Draw }

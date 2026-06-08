using UnityEngine;

public class NH_SurvivalStatus : MonoBehaviour
{
    [Header("Survival Meters")]
    [Range(0f, 100f)] public float reputation = 50f;
    [Range(0f, 100f)] public float respect = 0f;
    [Range(0f, 100f)] public float fear = 0f;
    [Range(0f, 100f)] public float debt = 0f;
    [Range(0f, 100f)] public float protection = 0f;
    [Range(0f, 100f)] public float snitchRisk = 0f;
    [Range(0f, 100f)] public float routineCompliance = 80f;

    public bool HasProtection => protection > 30f;
    public bool IsTargeted => fear > 70f || snitchRisk > 60f;
    public bool IsLowProfile => reputation < 30f && fear < 20f;

    public void ModifyReputation(float delta) => reputation = Mathf.Clamp(reputation + delta, 0f, 100f);
    public void ModifyRespect(float delta) => respect = Mathf.Clamp(respect + delta, 0f, 100f);
    public void ModifyFear(float delta) => fear = Mathf.Clamp(fear + delta, 0f, 100f);
    public void AddDebt(float amount) => debt = Mathf.Min(100f, debt + amount);
    public void PayDebt(float amount) => debt = Mathf.Max(0f, debt - amount);
}

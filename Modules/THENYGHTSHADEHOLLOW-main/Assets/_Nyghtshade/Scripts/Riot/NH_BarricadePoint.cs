using UnityEngine;

public class NH_BarricadePoint : NH_InteractableBase
{
    [Header("Barricade")]
    [Range(0f, 100f)] public float strength = 50f;
    public float breachTime = 15f;
    public float noiseGenerated = 60f;
    public int requiredResponseLevel = 2;
    public bool isActive;

    public override string DisplayName => "Barricade";
    public override string InteractVerb => isActive ? "Sabotage" : "Reinforce";

    public override void Interact(NH_PlayerStats player)
    {
        if (isActive)
            SabotageBarricade();
        else
            ReinforceBarricade();
    }

    public void ReinforceBarricade()
    {
        isActive = true;
        strength = Mathf.Min(100f, strength + 25f);
        NH_CrowdNoiseSystem.Instance?.PulseNoise(noiseGenerated, 5f);
        Debug.Log("[NH] Barricade reinforced.");
    }

    public void SabotageBarricade()
    {
        strength = Mathf.Max(0f, strength - 40f);
        if (strength <= 0f) BreachBarricade();
    }

    public void BreachBarricade()
    {
        isActive = false;
        strength = 0f;
        NH_CrowdNoiseSystem.Instance?.PulseNoise(noiseGenerated, 5f);
        NH_GuardSuspicionSystem.Instance?.AddSuspicion(20f, "barricade_breach");
        Debug.Log("[NH] Barricade breached.");
    }
}

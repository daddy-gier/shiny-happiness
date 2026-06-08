using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NH_GasExposureZone : MonoBehaviour
{
    [Range(0f, 100f)] public float gasIntensity = 80f;
    public float staminaDrainRate = 10f;
    public float stressAddRate = 5f;
    public bool isActive;

    void Awake() => GetComponent<Collider>().isTrigger = true;

    void OnTriggerStay(Collider other)
    {
        if (!isActive) return;
        var stats = other.GetComponentInParent<NH_PlayerStats>();
        if (stats == null) return;

        stats.DrainStamina(staminaDrainRate * Time.deltaTime);
        stats.AddStress(stressAddRate * Time.deltaTime);
    }

    public void Activate() => isActive = true;
    public void Deactivate() => isActive = false;
}

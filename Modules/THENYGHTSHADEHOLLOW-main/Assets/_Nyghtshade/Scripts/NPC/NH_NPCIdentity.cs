using UnityEngine;

public class NH_NPCIdentity : MonoBehaviour
{
    [Header("Identity")]
    public string npcId;
    public string displayName;
    public NH_NPCRole role;
    public string factionId;

    [Header("Personality")]
    [Range(0f, 100f)] public float courage = 50f;
    [Range(0f, 100f)] public float aggression = 30f;
    [Range(0f, 100f)] public float corruption = 0f;
    [Range(0f, 100f)] public float loyalty = 50f;

    [Header("Status")]
    public bool isAlive = true;
    public bool isConscious = true;
    public bool isCritical;
}

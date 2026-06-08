using System;

[Serializable]
public class NH_FactionStanding
{
    public string factionId;
    [UnityEngine.Range(-100f, 100f)] public float respect;
    [UnityEngine.Range(0f, 100f)] public float fear;
    [UnityEngine.Range(0f, 100f)] public float trust;
    [UnityEngine.Range(-100f, 100f)] public float debt;
    [UnityEngine.Range(0f, 100f)] public float heat;
    public bool isProtected;
    public bool isTargeted;
    public bool isHostile;

    public NH_FactionStanding(string id)
    {
        factionId = id;
        respect = 0f;
    }

    public void ModifyRespect(float delta) => respect = UnityEngine.Mathf.Clamp(respect + delta, -100f, 100f);
    public void ModifyFear(float delta) => fear = UnityEngine.Mathf.Clamp01((fear + delta) / 100f) * 100f;
    public void ModifyTrust(float delta) => trust = UnityEngine.Mathf.Clamp(trust + delta, 0f, 100f);
    public void ModifyDebt(float delta) => debt = UnityEngine.Mathf.Clamp(debt + delta, -100f, 100f);
}

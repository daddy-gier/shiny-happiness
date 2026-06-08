using UnityEngine;

[CreateAssetMenu(fileName = "NewFaction", menuName = "Nyghtshade/Faction Definition")]
public class NH_FactionDefinition : ScriptableObject
{
    public string factionId;
    public string displayName;
    [TextArea] public string description;
    public Color factionColor = Color.white;
    public bool isInmateFaction;
    public bool isGuardFaction;
    public bool isInstitutional;
    public string[] enemyFactionIds;
    public string[] allyFactionIds;
    public string primaryTerritoryZone;
}

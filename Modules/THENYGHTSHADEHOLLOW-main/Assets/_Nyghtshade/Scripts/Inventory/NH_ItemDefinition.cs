using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Nyghtshade/Item Definition")]
public class NH_ItemDefinition : ScriptableObject
{
    [Header("Identity")]
    public string itemId;
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Category & Risk")]
    public NH_ItemHeatCategory heatCategory;
    [Range(0f, 100f)] public float heatValue;
    public bool isQuestItem;
    public bool isEvidence;
    public bool canBeConfiscated = true;

    [Header("Economy")]
    public float commissaryValue = 1f;
    public float barterValue = 1f;
    public float blackMarketValue = 0f;

    [Header("Stack")]
    public bool stackable;
    public int maxStack = 1;
}

using System;
using System.Collections.Generic;

[Serializable]
public class NH_SaveData
{
    public float playerPosX, playerPosY, playerPosZ;
    public float playerRotY;
    public float health, stamina, stress, suspicion;
    public int currentDay;
    public string currentScheduleBlock;

    public List<string> inventoryItemIds = new List<string>();
    public List<int> inventoryQuantities = new List<int>();

    public float globalHeat;
    public float globalSuspicion;
    public bool lockdownActive;
    public string lockdownReason;

    public bool riotActive;
    public int riotPhase;
    public float riotThreat;

    public List<string> storyFlags = new List<string>();
    public List<string> questIds = new List<string>();
    public List<int> questStates = new List<int>();

    public List<string> factionIds = new List<string>();
    public List<float> factionRespect = new List<float>();

    public List<string> graveClues = new List<string>();
    public float graveMysteryDepth;

    public float fightClubReputation;
    public int playerFightRank;

    public string saveDateTimeUtc;
}

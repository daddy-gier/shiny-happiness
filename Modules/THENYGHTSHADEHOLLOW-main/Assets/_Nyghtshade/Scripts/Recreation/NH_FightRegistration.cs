using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_FightRegistration : NH_InteractableBase
{
    public string fightClubId = "mma_warehouse";
    public List<string> availableOpponentIds = new List<string>();

    public override string DisplayName => "Fight Registration Board";
    public override string InteractVerb => "Sign Up";

    public override void Interact(NH_PlayerStats player)
    {
        Debug.Log("[NH] Fight registration accessed.");
        NH_MMAFightClubSystem.Instance?.RegisterFighter(player);
    }
}

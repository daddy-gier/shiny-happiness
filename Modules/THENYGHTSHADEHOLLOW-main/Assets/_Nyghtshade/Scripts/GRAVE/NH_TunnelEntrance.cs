using UnityEngine;

public class NH_TunnelEntrance : NH_InteractableBase
{
    [Header("Tunnel Config")]
    public string tunnelSectionId;
    public bool requiresFlag;
    public string accessFlag = "grave_tunnel_unlocked";
    public Transform enterPoint;

    public override string DisplayName => "Tunnel Entrance";
    public override string InteractVerb => "Enter";

    public override bool CanInteract(NH_PlayerStats player)
    {
        if (requiresFlag && !NH_StoryFlagManager.Instance.HasFlag(accessFlag)) return false;
        return base.CanInteract(player);
    }

    public override void Interact(NH_PlayerStats player)
    {
        NH_GRAVEManager.Instance?.EnterTunnel(player, tunnelSectionId);

        if (enterPoint != null)
            player.GetComponent<NH_PlayerController>()?.Teleport(enterPoint.position, enterPoint.rotation);
    }
}

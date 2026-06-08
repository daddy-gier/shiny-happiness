using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_TunnelDiscoveryMap : MonoBehaviour
{
    [Serializable]
    public class TunnelSection
    {
        public string sectionId;
        public string displayName;
        public bool isDiscovered;
        public bool isFlooded;
        public bool hasEscapeRoute;
        public bool hasEvidence;
        public string requiredFlag;
    }

    public List<TunnelSection> sections = new List<TunnelSection>();

    public event Action<string> OnSectionDiscovered;

    public TunnelSection GetSection(string sectionId) => sections.Find(s => s.sectionId == sectionId);

    public void DiscoverSection(string sectionId)
    {
        var section = GetSection(sectionId);
        if (section == null || section.isDiscovered) return;

        section.isDiscovered = true;
        NH_StoryFlagManager.Instance.SetFlag($"tunnel_discovered_{sectionId}");
        OnSectionDiscovered?.Invoke(sectionId);
    }
}

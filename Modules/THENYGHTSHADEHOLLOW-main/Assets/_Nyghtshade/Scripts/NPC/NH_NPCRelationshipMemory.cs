using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_NPCRelationshipMemory : MonoBehaviour
{
    [Serializable]
    public class NPCMemory
    {
        public string targetId;
        [Range(-100f, 100f)] public float disposition;
        public bool knowsIsSnitch;
        public bool owesDebt;
        public bool hasProtectionAgreement;
        public string lastInteraction;
    }

    public List<NPCMemory> memories = new List<NPCMemory>();

    public NPCMemory GetMemory(string targetId)
    {
        var mem = memories.Find(m => m.targetId == targetId);
        if (mem == null)
        {
            mem = new NPCMemory { targetId = targetId };
            memories.Add(mem);
        }
        return mem;
    }

    public void RecordInteraction(string targetId, float dispositionDelta, string note)
    {
        var mem = GetMemory(targetId);
        mem.disposition = Mathf.Clamp(mem.disposition + dispositionDelta, -100f, 100f);
        mem.lastInteraction = note;
    }
}

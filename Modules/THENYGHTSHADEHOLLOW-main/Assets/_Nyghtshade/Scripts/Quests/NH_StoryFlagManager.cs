using System;
using System.Collections.Generic;
using UnityEngine;

public class NH_StoryFlagManager : MonoBehaviour
{
    public static NH_StoryFlagManager Instance { get; private set; }

    private HashSet<string> _flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    public event Action<string> OnFlagSet;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return;
        _flags.Add(flag);
        OnFlagSet?.Invoke(flag);
    }

    public void ClearFlag(string flag) => _flags.Remove(flag);

    public bool HasFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return true;
        return _flags.Contains(flag);
    }

    public List<string> GetAllFlags() => new List<string>(_flags);

    public void LoadFlags(List<string> flags)
    {
        _flags.Clear();
        foreach (var f in flags) _flags.Add(f);
    }
}

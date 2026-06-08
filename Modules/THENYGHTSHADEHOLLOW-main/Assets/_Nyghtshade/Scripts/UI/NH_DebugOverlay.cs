using UnityEngine;

public class NH_DebugOverlay : MonoBehaviour
{
    public bool showFPS = true;
    public bool showSystems = true;

    private float _fpsTimer;
    private float _fps;
    private GUIStyle _style;

    void Start()
    {
        _style = new GUIStyle();
        _style.fontSize = 16;
        _style.normal.textColor = Color.lime;
    }

    void Update()
    {
        _fpsTimer += Time.deltaTime;
        if (_fpsTimer >= 0.5f) { _fps = 1f / Time.deltaTime; _fpsTimer = 0f; }
    }

    void OnGUI()
    {
        if (!gameObject.activeSelf) return;

        int y = 10;
        if (showFPS) GUI.Label(new Rect(10, y, 200, 20), $"FPS: {_fps:F0}", _style); y += 22;

        if (showSystems)
        {
            var stats = FindFirstObjectByType<NH_PlayerStats>();
            if (stats != null)
            {
                GUI.Label(new Rect(10, y, 300, 20), $"HP:{stats.Health:F0} STA:{stats.Stamina:F0} STR:{stats.Stress:F0} SUS:{stats.SuspicionExposure:F0}", _style); y += 22;
                GUI.Label(new Rect(10, y, 300, 20), $"Zone: {stats.CurrentZone} | Schedule: {stats.CurrentScheduleState}", _style); y += 22;
            }
            var susp = NH_GuardSuspicionSystem.Instance;
            if (susp != null) { GUI.Label(new Rect(10, y, 300, 20), $"Guard Suspicion: {susp.globalSuspicion:F0} [{susp.responseLevel}]", _style); y += 22; }

            var lock_ = NH_LockdownManager.Instance;
            if (lock_?.isLockdownActive == true) { GUI.Label(new Rect(10, y, 300, 20), $"LOCKDOWN: {lock_.lockdownReason}", _style); y += 22; }

            var riot = NH_RiotManager.Instance;
            if (riot?.isActive == true) { GUI.Label(new Rect(10, y, 300, 20), $"RIOT: {riot.currentPhase}", _style); y += 22; }
        }
    }
}

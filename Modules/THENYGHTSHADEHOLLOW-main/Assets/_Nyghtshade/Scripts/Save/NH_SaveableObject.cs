using UnityEngine;

public abstract class NH_SaveableObject : MonoBehaviour, NH_ISaveable
{
    [SerializeField] private string saveId;

    void OnValidate()
    {
        if (string.IsNullOrEmpty(saveId))
            saveId = System.Guid.NewGuid().ToString().Substring(0, 8);
    }

    public string SaveId => saveId;
    public abstract object CaptureState();
    public abstract void RestoreState(object state);
}

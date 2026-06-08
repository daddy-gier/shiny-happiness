public interface NH_ISaveable
{
    string SaveId { get; }
    object CaptureState();
    void RestoreState(object state);
}

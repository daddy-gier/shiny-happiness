using UnityEngine;

public class NH_PlayerSpawnPoint : MonoBehaviour
{
    public bool isDefaultSpawn = true;
    public string spawnId = "default";

    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
    }
}

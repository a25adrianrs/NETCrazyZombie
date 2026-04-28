using UnityEngine;
using Unity.Netcode;
public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    private NetworkObject netObject;
    void Start()
    {
        netObject = GetComponent<NetworkObject>();
        // SOLO el servidor programa la destrucción
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            Invoke(nameof(DestroyAfterTime), lifeTime);
        }
    }

    void DestroyAfterTime()
    {
        if (netObject != null && netObject.IsSpawned)
        {
            netObject.Despawn(); // Despawner en la red
        }
        // Destruimos el objeto localmente
        Destroy(gameObject);
    }
}

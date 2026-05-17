using UnityEngine;
using Unity.Netcode;

// Destruye el objeto después de un tiempo de vida definido, despawneando también en la red.
public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f; // Tiempo en segundos antes de destruir el objeto.

    private NetworkObject netObject;

    void Start()
    {
        netObject = GetComponent<NetworkObject>();
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            // Solo el servidor programa y ejecuta la destrucción en red.
            Invoke(nameof(DestroyAfterTime), lifeTime);
        }
    }

    void DestroyAfterTime()
    {
        if (netObject != null && netObject.IsSpawned)
        {
            netObject.Despawn(); // Elimina el objeto sincronizado de la red.
        }

        Destroy(gameObject); // Destruye el GameObject local.
    }
}

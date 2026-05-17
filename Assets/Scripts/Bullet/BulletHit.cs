using UnityEngine;
using Unity.Netcode;

// Maneja el impacto de la bala cuando colisiona con algo.
// El servidor decide qué hacer y despawnea la bala de la red.
public class BulletHit : MonoBehaviour
{
    [SerializeField] GameObject particle; // Efecto visual que se reproduce al impactar.

    private bool hasHit = false; // Evita procesar el mismo impacto varias veces.

    void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsServer) return; // Solo el servidor maneja la lógica de impacto.
        if (hasHit) return; // Ignora impactos adicionales después del primero.

        hasHit = true;
        Debug.Log("Impacto con: " + collision.gameObject.name);

        if (particle != null)
        {
            Instantiate(particle, transform.position, Quaternion.identity); // Crea el efecto de impacto.
        }

        NetworkObject netObject = GetComponent<NetworkObject>();
        if (netObject != null && netObject.IsSpawned)
        {
            netObject.Despawn(); // Elimina la bala sincronizada en red.
        }
    }
}

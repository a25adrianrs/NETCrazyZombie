using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Aplica daño a un jugador cuando este objeto colisiona contra él.
// La lógica de daño se ejecuta únicamente en el servidor para mantener la coherencia de la red.
public class DealDamageContact : MonoBehaviour
{
    public int PLAYER_DAMAGE = 10; // Cantidad de salud que se resta al jugador en contacto.

    private ulong ownerClientId; // Identificador del cliente propietario del objeto, si se utiliza.

    public void SetOwnerClientId(ulong clientId)
    {
        ownerClientId = clientId; // Método para asignar el propietario de este objeto.
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsServer) return; // Solo el servidor procesa el daño.

        if (collision.gameObject.CompareTag("Player"))
        {
            var health = collision.gameObject.GetComponent<HealthPlayer>();

            if (health != null)
            {
                Debug.Log("Collision " + collision.gameObject.name);
                health.TakenDamage(PLAYER_DAMAGE); // Aplica daño al jugador.
            }
        }
    }
}

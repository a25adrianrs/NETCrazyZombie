using UnityEngine;
using Unity.Netcode;

// Aplica daño al jugador mientras el zombi está en contacto continuo con él.
// Solo el servidor debe modificar la salud del jugador.
public class ZombieBite : NetworkBehaviour
{
    public const int PLAYER_DAMAGE = 10; // Daño aplicado por cada frame de colisión.

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var health = other.gameObject.GetComponent<HealthPlayer>();
            if (IsServer && health != null)
            {
                // Llama al método de daño del jugador en el servidor.
                health.TakenDamage(PLAYER_DAMAGE);
            }
        }
    }
}

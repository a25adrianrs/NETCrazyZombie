using UnityEngine;
using Unity.Netcode;

// Aplica el efecto del powerup al jugador que lo recoge.
// Solo el servidor procesa la colisión y realiza los cambios de salud en red.
public class PowerUpApply : NetworkBehaviour
{
    const int POWER = 50; // Cantidad de salud que restaura el powerup.

    [SerializeField] AudioClip clip; // Sonido que se reproduce al recoger el powerup.

    void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.CompareTag("Player"))
            {
                var health = other.gameObject.GetComponent<HealthPlayer>();

                if (health != null)
                {
                    // Llama a TakenDamage con valor negativo para curar al jugador.
                    health.TakenDamage(-POWER);
                }

                AudioSource.PlayClipAtPoint(clip, transform.position); // Reproduce sonido localmente en la posición del powerup.

                GetComponent<NetworkObject>().Despawn(); // Elimina el objeto de la red.
                Destroy(gameObject); // Destruye el GameObject local.
            }
        }
    }
}

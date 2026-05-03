using UnityEngine;
using Unity.Netcode;
public class BulletHit : MonoBehaviour
{
    [SerializeField] GameObject particle;

    private bool hasHit = false;
    void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsServer) return; // Solo el servidor maneja la lógica de colisión   

        if (hasHit) return; // Evita procesar múltiples colisiones
        hasHit = true; // Marca que ya se ha procesado una colisión

        Debug.Log("Impacto con: " + collision.gameObject.name);

        //Daño al jugador si colisiona con una bala
        /* if (collision.gameObject.CompareTag("Player"))
         {
             //collision.gameObject.SendMessage("ApplyDamage", damage);
             // Obtenemos la vida del personaje y la guardamos en la variable health

             var health = collision.gameObject.GetComponent<HealthPlayer>();
             if (health != null)
             {
                 // Si la variable health es distinta de null entonces llamamos al metodo TakeDamage y le pasamos el daño
                 health.TakenDamage(damage);
             }
         }*/

        if (particle != null)
        {
            // Instanciamos el efecto de impacto
            Instantiate(particle, transform.position, Quaternion.identity);
        }

        //Destruimos el proyectil en la red
        NetworkObject netObject = GetComponent<NetworkObject>();
        if (netObject != null && netObject.IsSpawned)
        {
            netObject.Despawn(); // Despawner en la red
        }

        // Destruimos el proyectil
        //gameObject.SetActive(false);
    }
}

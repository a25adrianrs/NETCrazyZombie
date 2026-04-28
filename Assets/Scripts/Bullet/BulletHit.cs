using UnityEngine;
using Unity.Netcode;
public class BulletHit : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] int damage = 10;
    void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsServer) return; // Solo el servidor maneja la lógica de colisión   
        Debug.Log("Impacto con: " + collision.gameObject.name);
        //Daño al jugador si colisiona con una bala
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("ApplyDamage", damage);
        }

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

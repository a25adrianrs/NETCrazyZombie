using UnityEngine;
using Unity.Netcode;

// Gestiona el daño y la muerte de un zombi cuando recibe impactos de balas.
// Solo el servidor procesa las colisiones y decide cuándo eliminar el zombi.
public class ZombieDamage : NetworkBehaviour
{
    const int HITS_TO_DIE = 3; // Cantidad de golpes necesarios para matar al zombi.
    int hitCount; // Contador de impactos acumulados.

    private GameObject zombieManager; // Referencia al objeto que controla el spawn de zombis.

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        zombieManager = GameObject.Find("ZombieSpawner"); // Busca el objeto SpawnManager en la escena.
    }

    void OnCollisionEnter(Collision other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                hitCount++; // Aumenta el contador de golpes del zombi.

                if (hitCount == HITS_TO_DIE)
                {
                    // Llama al método de destrucción en el servidor usando el NetworkObject de este zombi.
                    zombieManager.GetComponent<ZombieSpawner>().DestroyZombieRpc(gameObject.GetComponent<NetworkObject>());
                }
            }
        }
    }
}

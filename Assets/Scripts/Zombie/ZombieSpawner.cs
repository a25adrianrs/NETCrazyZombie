using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System;

// Maneja el spawn y la destrucción de zombis en el servidor utilizando Unity Netcode.
// El servidor controla cuándo aparecen los enemigos y cuántos hay al mismo tiempo.
public class ZombieSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] GameObject zombie; // Prefab del zombi a instanciar en la escena.

    [Header("Settings")]
    [SerializeField] float spawnDelay; // Intervalo en segundos entre cada intento de spawn.
    [SerializeField] int zombieMax; // Número máximo de zombis permitidos al mismo tiempo.

    int numZombies = 0; // Contador actual de zombis activos.

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Solo el servidor debe ejecutar la lógica de spawn en red.
            // InvokeRepeating llamará SpawnZombieRpc repetidamente después de 2 segundos.
            InvokeRepeating(nameof(SpawnZombieRpc), 2f, spawnDelay);
        }
    }

    [Rpc(SendTo.Server)]
    public void DestroyZombieRpc(NetworkObjectReference networkObjectReference)
    {
        // Este RPC se usa para pedir al servidor que destruya un zombi en la red.
        NetworkObject target = networkObjectReference;
        target.Despawn(); // Despawner el objeto en Netcode.
        Destroy(target.gameObject); // Elimina el GameObject local del servidor.
        numZombies--; // Actualiza el conteo de zombis.
    }

    [Rpc(SendTo.Server)]
    private void SpawnZombieRpc()
    {
        if (!IsServer) return;

        // Solo se instancian nuevos zombis si no hemos alcanzado el máximo permitido.
        if (numZombies < zombieMax)
        {
            GameObject enemy = Instantiate(zombie, transform.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(); // Spawnea el zombi en todos los clientes.
            numZombies++;
        }
    }
}

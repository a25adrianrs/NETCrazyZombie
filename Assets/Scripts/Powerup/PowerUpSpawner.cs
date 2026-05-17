using System.Collections;
using UnityEngine;
using Unity.Netcode;

// Gestiona el spawn de un powerup en el servidor.
// Solo se debe crear un powerup a la vez y se sincroniza a todos los clientes.
public class PowerUpSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] GameObject prefab; // Prefab del powerup a instanciar.
    [SerializeField] Transform[] spawnPoints; // Puntos posibles donde puede aparecer el powerup.

    [Header("Settings")]
    [SerializeField] float delay; // Intervalo entre intentos de spawn.

    GameObject powerUp; // Referencia al powerup actualmente activo.

    public override void OnNetworkSpawn()
    {
        // Inicia el spawn periódico en el servidor.
        InvokeRepeating(nameof(SpawnRpc), 2f, delay);
    }

    [Rpc(SendTo.Server)]
    private void SpawnRpc()
    {
        if (IsServer && powerUp == null)
        {
            Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            powerUp = Instantiate(prefab, position, Quaternion.identity); // Instancia el powerup en el servidor.
            powerUp.GetComponent<NetworkObject>().Spawn(); // Sincroniza el powerup con todos los clientes.
        }
    }
}

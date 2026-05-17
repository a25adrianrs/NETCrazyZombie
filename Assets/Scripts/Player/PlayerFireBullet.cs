using System;
using UnityEngine;
using Unity.Netcode;

// Controla el disparo de proyectiles en red.
// El propietario local genera la bala visual y pide al servidor que cree la bala real.
public class PlayerFireBullet : NetworkBehaviour
{
    [SerializeField] GameObject proyectile; // Prefab de la bala que se instanciará en el servidor.
    [SerializeField] GameObject clientBullet; // Prefab de la bala visual local.
    [SerializeField] Transform shootPoint; // Punto de origen del disparo.

    void Update()
    {
        if (!IsOwner) return; // Solo el cliente local del jugador dispara.
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(clientBullet, shootPoint.position, shootPoint.rotation); // Bala visual local inmediata.
            FireServerRPC(shootPoint.position, shootPoint.rotation); // Pide al servidor que cree la bala real.
        }
    }

    [Rpc(SendTo.Server)]
    void FireServerRPC(Vector3 pos, Quaternion rot)
    {
        GameObject bullet = Instantiate(proyectile, pos, rot); // Instancia la bala real en el servidor.
        FireClientRPC(shootPoint.position, shootPoint.rotation); // Informa a todos los clientes del disparo.
    }

    [Rpc(SendTo.ClientsAndHost)]
    void FireClientRPC(Vector3 pos, Quaternion rot)
    {
        if (IsOwner) return; // El cliente que disparó ya creó su bala visual, no necesita otra.
        Instantiate(clientBullet, pos, rot); // Crea una bala visual para los demás clientes.
    }
}

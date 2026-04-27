using System;
using UnityEngine;
using Unity.Netcode;

public class PlayerFireBullet : NetworkBehaviour
{
    //[SerializeField] GameObject proyectile;

    [SerializeField] GameObject serverBullet;
    [SerializeField] GameObject clientBullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] float bulletSpeed = 20f;


    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetButtonDown("Fire1"))
        {
            // Si el jugador es el host, dispara la bala del host, de lo contrario dispara la bala del cliente
            ShootServerRpc(shootPoint.position, shootPoint.forward);
            // Crea la bala visual y la mueve hacia adelante
            SpawnClientBullet(shootPoint.position, shootPoint.forward);
            //FireRpc();
        }
    }

    /*[Rpc(SendTo.Server)]
    void FireRpc()
    {
        GameObject bala = Instantiate(proyectile, transform.position, transform.rotation);
        bala.GetComponent<NetworkObject>().Spawn(true);
    }*/
    void SpawnClientBullet(Vector3 pos, Vector3 dir)
    {
        // Solo el cliente que es el propietario del jugador dispara la bala visual
        GameObject b = Instantiate(clientBullet, pos, Quaternion.identity);
        // Orienta la bala en la dirección del disparo
        b.transform.forward = dir;
        // Aplica velocidad a la bala
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // La bala visual se mueve pero no se sincroniza en la red, es solo para feedback visual del disparo
            rb.linearVelocity = dir * bulletSpeed;
        }
    }

    [Rpc(SendTo.Server)]
    void ShootServerRpc(Vector3 pos, Vector3 dir)
    {
        // Solo el servidor crea la bala real que se sincroniza en la red
        GameObject b = Instantiate(serverBullet, pos, Quaternion.identity);
        // Orienta la bala en la dirección del disparo
        b.transform.forward = dir;
        // Aplica velocidad a la bala
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * bulletSpeed;
        }
        // Spawnea la bala en la red para que todos los clientes la vean
        b.GetComponent<NetworkObject>().Spawn();
    }
}

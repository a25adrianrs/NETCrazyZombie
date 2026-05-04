using System;
using UnityEngine;
using Unity.Netcode;

public class PlayerFireBullet : NetworkBehaviour
{
    [SerializeField] GameObject proyectile;
    [SerializeField] GameObject clientBullet;
    [SerializeField] Transform shootPoint;

    /* [SerializeField] GameObject serverBullet;
     [SerializeField] GameObject clientBullet;
     [SerializeField] Transform shootPoint;
     [SerializeField] float bulletSpeed = 20f;*/


    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(clientBullet, shootPoint.position, shootPoint.rotation);
            FireServerRPC(shootPoint.position, shootPoint.rotation);

            //FireRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void FireServerRPC(Vector3 pos, Quaternion rot)
    {
        GameObject bullet = Instantiate(proyectile, pos, rot);
        FireClientRPC(shootPoint.position, shootPoint.rotation);


    }

    [Rpc(SendTo.ClientsAndHost)]
    void FireClientRPC(Vector3 pos, Quaternion rot)
    {
        if (IsOwner) return; // El cliente que disparó ya instanció su bala visual, no necesita otra
        Instantiate(clientBullet, pos, rot);
    }

    /*[Rpc(SendTo.Server)]
    void FireRpc()
    {
        GameObject bala = Instantiate(proyectile, transform.position, transform.rotation);
        bala.GetComponent<NetworkObject>().Spawn(true);
    }*/
    /* void SpawnClientBullet(Vector3 pos, Vector3 dir)
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
     }*/
}

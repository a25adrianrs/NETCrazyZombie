using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class DealDamageContact : MonoBehaviour
{
    public int PLAYER_DAMAGE = 10;

    private ulong ownerClientId;
    public void SetOwnerClientId(ulong clientId)
    {
        ownerClientId = clientId;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("ApplyDamage", PLAYER_DAMAGE);
        }
    }
}

using UnityEngine;
using Unity.Netcode;

public class ZombieBite : NetworkBehaviour
{
    public const int PLAYER_DAMAGE = 10;
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var health = other.gameObject.GetComponent<HealthPlayer>();
            if (IsServer)
            {
                health.TakenDamage(PLAYER_DAMAGE);
            }
        }
    }

}

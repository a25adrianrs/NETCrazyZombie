using UnityEngine;
using Unity.Netcode;

public class PowerUpApply : NetworkBehaviour
{
    const int POWER = 50;

    [SerializeField] AudioClip clip;

    void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.CompareTag("Player"))
            {
                var health = other.gameObject.GetComponent<HealthPlayer>();

                if (health != null)
                {
                    health.TakenDamage(-POWER); // negativo = curar
                }

                AudioSource.PlayClipAtPoint(clip, transform.position);

                GetComponent<NetworkObject>().Despawn();
                Destroy(gameObject);
            }
        }
    }
}

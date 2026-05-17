using UnityEngine;
using Unity.Netcode;

// Activa o desactiva la interfaz de usuario del jugador según si este objeto es el owner local.
public class PlayerUI : NetworkBehaviour
{
    public GameObject playerUI; // Referencia al Canvas UI del jugador.

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            playerUI.SetActive(false); // Oculta la UI para jugadores que no son el owner.
        }
        else
        {
            playerUI.SetActive(true); // Muestra la UI solo al dueño local.
        }
    }
}

using UnityEngine;
using Unity.Netcode;

// Activa la cámara y el audio solo para el jugador propietario del objeto de red.
// Los demás clientes tienen el mismo prefab, pero no deben usar esta cámara.
public class CameraOwnerComponentManager : NetworkBehaviour
{
    [SerializeField] private Camera _camera; // Cámara del jugador que se activará si es el owner.
    [SerializeField] private AudioListener _audioListener; // AudioListener del jugador que se activa solo para el owner.

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; } // Solo el propietario local debe ejecutar el código siguiente.

        _camera.enabled = true; // Activa la cámara solo para este jugador.
        _audioListener.enabled = true; // Activa el audio solo para este jugador.
    }
}
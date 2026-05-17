using System;
using UnityEngine;
using Unity.Netcode;

// Maneja el giro de la cámara con el ratón para el jugador local en un juego multijugador.
// El cliente propietario lee Input y envía la rotación al servidor con un RPC.
public class CameraMouseLook : NetworkBehaviour
{
    const float CLAMP_MIN = -45.0f;
    const float CLAMP_MAX = 45.0f;

    [SerializeField] float lookSensitivity; // Sensibilidad de la rotación horizontal.
    Vector2 rotation = Vector2.zero; // Rotación objetivo acumulada.
    Vector2 smoothRot = Vector2.zero; // Rotación suavizada para la cámara.
    Vector2 velRot = Vector2.zero; // Velocidad usada por SmoothDamp.

    GameObject player; // Referencia al objeto padre que representa al jugador.

    public override void OnNetworkSpawn()
    {
        player = transform.parent.gameObject; // El objeto padre es el cuerpo del jugador.
    }

    void Update()
    {
        if (IsOwner)
        {
            float axis_x = Input.GetAxis("Mouse X"); // Movimiento horizontal del ratón.
            rotation.y += Input.GetAxis("Mouse Y"); // Movimiento vertical del ratón.
            rotation.y = Mathf.Clamp(rotation.y, CLAMP_MIN, CLAMP_MAX); // Limita el ángulo vertical.
            smoothRot.y = Mathf.SmoothDamp(smoothRot.y, rotation.y, ref velRot.y, 0.1f); // Suaviza el movimiento.

            LookAroundRpc(smoothRot, axis_x); // Envía la rotación al servidor.
        }
    }

    [Rpc(SendTo.Server)]
    void LookAroundRpc(Vector2 smoothRot, float axis_x)
    {
        transform.localEulerAngles = new Vector3(-smoothRot.y, 0, 0); // Mueve la cámara arriba/abajo.
        player.transform.RotateAround(transform.position, Vector3.up, axis_x * lookSensitivity); // Gira al jugador a izquierda/derecha.
    }
}

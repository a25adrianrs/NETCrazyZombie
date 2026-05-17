using UnityEngine;
using Unity.Netcode;

// Mueve la bala hacia adelante en cada frame.
// No maneja colisiones ni destrucción en esta clase, solo el movimiento visual.
public class BulletMove : MonoBehaviour
{
    public float speed = 10f; // Velocidad a la que avanza la bala.

    void Update()
    {
        // Avanza la bala en la dirección local "forward".
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

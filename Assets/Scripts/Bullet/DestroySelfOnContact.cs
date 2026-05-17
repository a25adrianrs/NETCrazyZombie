using UnityEngine;

// Destruye el objeto padre cuando este elemento colisiona con otro.
// Se usa típicamente para destruir un proyectil compuesto de varios objetos.
public class DestroySelfOnContact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(transform.parent.gameObject); // Elimina el objeto padre inmediatamente.
    }
}

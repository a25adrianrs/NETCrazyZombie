using UnityEngine;

// Destruye el efecto visual de impacto después de un tiempo corto.
// Esto evita que los efectos queden flotando en escena indefinidamente.
public class DestroyHitEffect : MonoBehaviour
{
    const float TIME = 1; // Tiempo en segundos antes de destruir el efecto.
    float timer; // Contador acumulado de tiempo desde que el objeto se creó.

    void Update()
    {
        timer += Time.deltaTime; // Incrementa el temporizador con el tiempo entre frames.
        if (timer >= TIME)
        {
            Destroy(gameObject); // Elimina el GameObject cuando se supera el tiempo.
        }
    }
}

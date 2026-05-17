using UnityEngine;

// Abre una puerta cuando el jugador entra en su trigger.
public class DoorOpen : MonoBehaviour
{
    [SerializeField] Animator anim; // Animator que controla la animación de la puerta.

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("Open", true); // Activa la animación de apertura.
        }
    }
}

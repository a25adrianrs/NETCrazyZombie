using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

// Controla las animaciones del zombi cuando persigue y ataca a un jugador.
// Usa NavMeshAgent para detener el movimiento y cambiar de estado de animación.
public class ZombieAnim : NetworkBehaviour
{
    [SerializeField] Animator anim; // Animator del zombi que controla las transiciones de animación.
    NavMeshAgent agent; // Componente que gestiona la navegación del zombi.

    public override void OnNetworkSpawn()
    {
        anim.SetBool("IsRunning", true); // Inicia el zombi en estado de correr.
        agent = GetComponent<NavMeshAgent>(); // Obtiene el agente de navegación del mismo GameObject.
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (agent != null && !agent.isStopped)
            {
                agent.SetDestination(transform.position); // Cancela el destino para detener al agente.
                agent.isStopped = true; // Pausa el movimiento del zombi.
            }

            anim.SetBool("IsAttacking", true); // Cambia la animación a ataque.
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("IsAttacking", false); // Deja de atacar cuando el jugador se separa.

            Invoke("ResumeAgent", 3f); // Reactiva el movimiento tras un retraso.
        }
    }

    void ResumeAgent()
    {
        agent.isStopped = false; // Vuelve a permitir que el agente siga el camino.
    }
}

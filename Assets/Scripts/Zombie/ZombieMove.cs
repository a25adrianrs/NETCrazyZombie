using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;

// Controla el movimiento de un zombi usando NavMeshAgent.
// El servidor decide la ruta al jugador más cercano y actualiza la posición del agente.
public class ZombieMove : NetworkBehaviour
{
    Transform target; // Objetivo actual al cual el zombi se dirige.

    NavMeshAgent agent; // Componente de navegación que calcula y sigue el camino.

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            agent = GetComponent<NavMeshAgent>();
            // Inicia la búsqueda de objetivos unos segundos después de spawnear.
            Invoke("FindTarget", 3);
        }
    }

    void Update()
    {
        if (!IsServer) return; // Solo el servidor controla el movimiento de los enemigos.

        FindTarget();

        if (!agent.isStopped && target != null)
        {
            // Establece la posición destino del agente hacia el jugador más cercano.
            agent.SetDestination(target.position);
        }
    }

    private void FindTarget()
    {
        target = GetNearestPlayer(); // Busca el jugador más cercano disponible.
    }

    private Transform GetNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            if (CanReachTarget(player.transform.position))
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlayer = player.transform;
                }
            }
        }

        return nearestPlayer;
    }

    bool CanReachTarget(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(targetPosition, path))
        {
            Debug.Log("path.status: " + path.status);
            // Devuelve true solo si existe un camino completo hacia el objetivo.
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }
}

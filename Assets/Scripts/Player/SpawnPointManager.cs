using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.Networking.Transport.Error;

// Controla los puntos de aparición de los jugadores.
// Este script elige un punto aleatorio distinto al último utilizado para evitar reaparecer siempre en el mismo lugar.
public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // Lista de ubicaciones posibles de spawn.

    private int lastSpawnIndex = -1; // Índice del último punto usado.

    public Vector3 GetRandomSpawnPoint()
    {
        int index;
        do
        {
            index = Random.Range(0, spawnPoints.Length); // Selecciona un índice aleatorio.
        } while (index == lastSpawnIndex); // Evita repetir el mismo punto consecutivamente.

        lastSpawnIndex = index; // Guarda el punto elegido para la próxima vez.
        return spawnPoints[index].position; // Devuelve la posición del punto de spawn.
    }
}

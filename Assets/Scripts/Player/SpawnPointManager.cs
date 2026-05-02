using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.Networking.Transport.Error;

public class SpawnPointManager : MonoBehaviour
{
    // La clase SpawnPointManager se encarga de gestionar los puntos de aparición de los jugadores en el juego.
    // Contiene un array de Transform llamado spawnPoints, que se puede configurar desde el inspector de Unity para definir las posiciones donde los jugadores pueden aparecer.
    // El método GetRandomSpawnPoint() se utiliza para obtener una posición de aparición aleatoria para un jugador. 
    // Este método utiliza NavMesh.SamplePosition 
    // para asegurarse de que la posición de aparición esté en una ubicación válida dentro del NavMesh, 
    // lo que es especialmente útil en juegos con terrenos irregulares o obstáculos.
    [SerializeField] private Transform[] spawnPoints;

    // Variable que se utiliza para evitar que el mismo punto de aparición se seleccione consecutivamente.
    private int lastSpawnIndex = -1;


    public Vector3 GetRandomSpawnPoint()
    {
        int index;
        // Mientras el índice generado aleatoriamente sea igual al índice del último punto de aparición utilizado, 
        // se seguirá generando un nuevo índice.
        do
        {
            index = Random.Range(0, spawnPoints.Length);

        } while (index == lastSpawnIndex);

        // Una vez que se obtiene un índice diferente al último utilizado, 
        // se actualiza la variable lastSpawnIndex con el nuevo índice para futuras comparaciones.
        lastSpawnIndex = index;

        // Finalmente, se devuelve la posición del punto de aparición correspondiente al índice seleccionado.
        return spawnPoints[index].position;
        /*NavMeshHit hit;
        if (NavMesh.SamplePosition(new Vector3(0, 0, 0), out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }*/
    }
}

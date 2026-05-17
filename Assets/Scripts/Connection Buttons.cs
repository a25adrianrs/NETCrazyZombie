using Unity.Netcode;
using UnityEngine;

// Este script expone botones para iniciar la sesión de red como Host, Cliente o Servidor.
// Se utiliza junto a Unity Netcode y llama directamente a NetworkManager.Singleton para controlar la conexión.
public class ConnectionButtons : MonoBehaviour
{
    // Inicia la sesión como Host usando NetworkManager.
    // El Host actúa como servidor y cliente al mismo tiempo.
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    // Inicia la sesión como Cliente usando NetworkManager.
    // El cliente se conecta a un servidor o host existente.
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    // Inicia la sesión como Servidor usando NetworkManager.
    // El servidor admite conexiones de clientes pero no controla una cámara o jugador local.
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}

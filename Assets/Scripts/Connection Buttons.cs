using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{

    // Mediante el metodo StartHost, StartClient y StartServer se llama a los metodos correspondientes del NetworkManager
    //  para iniciar la conexion como Host, Cliente o Servidor respectivamente.
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}

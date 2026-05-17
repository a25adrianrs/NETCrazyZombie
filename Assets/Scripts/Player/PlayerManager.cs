using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

// Administra el estado de un jugador en red: nombre, reaparecer, salud y estadísticas de spawns.
// Se sincronizan variables de red para que cada cliente vea información actualizada del jugador.
public class PlayerManager : NetworkBehaviour
{
    public NetworkVariable<int> spawns; // Contador de veces que el jugador se ha reaparecido.
    public NetworkVariable<FixedString128Bytes> username; // Nombre del jugador sincronizado en red.

    [SerializeField] TMP_Text m_UsernameLabel; // Etiqueta UI para mostrar el nombre del jugador.

    [SerializeField] private SpawnPointManager spawnPointManager; // Gestor de puntos de respawn.
    public TextMeshProUGUI txtHealth; // Texto UI para mostrar la salud actual.
    public TextMeshProUGUI txtSpawns; // Texto UI para mostrar las reaparecidas.

    private HealthPlayer healthPlayer; // Referencia al componente que maneja la salud.

    private void Awake()
    {
        username = new NetworkVariable<FixedString128Bytes>(Utilities.GetRandomUsername());
        healthPlayer = GetComponent<HealthPlayer>(); // Obtiene el componente de salud en el mismo GameObject.
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        spawns.OnValueChanged += OnSpawnsChanged; // Actualiza UI cuando cambian las reaparecidas.
        username.OnValueChanged += OnClientUsernameChanged; // Actualiza el nombre en la UI.

        healthPlayer.CurrentHealth.OnValueChanged += OnClientHealthChanged; // Escucha cambios de salud.
        healthPlayer.OnDie += Die; // Se suscribe al evento de muerte del jugador.

        ChangeNameRpc(Utilities.GetRandomUsername()); // Establece un nombre aleatorio.

        if (IsServer)
        {
            transform.position = spawnPointManager.GetRandomSpawnPoint(); // Respawnea el jugador en el servidor.
        }

        // Inicializa la UI con el valor actual de salud.
        OnClientHealthChanged(healthPlayer.CurrentHealth.Value, healthPlayer.CurrentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        username.OnValueChanged -= OnClientUsernameChanged; // Desuscribe del evento del nombre.
        spawns.OnValueChanged -= OnSpawnsChanged; // Desuscribe del evento de spawns.
        healthPlayer.CurrentHealth.OnValueChanged -= OnClientHealthChanged; // Desuscribe del evento de salud.
        healthPlayer.OnDie -= Die; // Desuscribe del evento de muerte.
    }

    private void OnClientUsernameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        m_UsernameLabel.text = newValue.ToString(); // Actualiza la etiqueta del nombre.
    }

    [Rpc(SendTo.Server)]
    public void ChangeNameRpc(FixedString128Bytes newValue)
    {
        if (!IsServer) return; // Solo el servidor modifica la variable de red.
        username.Value = newValue;
    }

    void OnClientHealthChanged(int previousHealth, int newHealth)
    {
        txtHealth.text = newHealth.ToString(); // Muestra la salud actual en UI.
    }

    void OnSpawnsChanged(int previousValue, int newValue)
    {
        txtSpawns.text = newValue.ToString(); // Actualiza el contador de respawns.
    }

    private void Die()
    {
        if (!IsServer) return; // Solo el servidor decide reaparecer.
        Respawn();
    }

    private void Respawn()
    {
        if (!IsServer) return;
        transform.position = spawnPointManager.GetRandomSpawnPoint(); // Ubica al jugador en un punto seguro.
        healthPlayer.ResetHealth(); // Restaura la vida del jugador.
        spawns.Value++; // Incrementa el contador de respawns.
    }
}

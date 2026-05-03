using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public class PlayerManager : NetworkBehaviour
{

    public NetworkVariable<int> spawns;
    public NetworkVariable<FixedString128Bytes> username;

    //[SerializeField] Image m_HealthBarImage;
    [SerializeField] TMP_Text m_UsernameLabel;

    //private GameObject playerSpawner;
    [SerializeField] private SpawnPointManager spawnPointManager;
    public TextMeshProUGUI txtHealth;

    public TextMeshProUGUI txtSpawns;

    private HealthPlayer healthPlayer;



    private void Awake()
    {

        username = new NetworkVariable<FixedString128Bytes>(Utilities.GetRandomUsername());
        //playerSpawner = GameObject.Find("PlayerSpawner");
        healthPlayer = GetComponent<HealthPlayer>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //health.OnValueChanged += OnClientHealthChanged;
        spawns.OnValueChanged += OnSpawnsChanged;
        username.OnValueChanged += OnClientUsernameChanged;

        // Mediante el componente HealthPlayer, accedemos a la variable de salud y 
        // nos suscribimos al evento OnValueChanged para actualizar la barra de salud en los clientes. 
        // Esto asegura que cada vez que la salud cambie, la interfaz de usuario se actualice correctamente 
        // para reflejar el nuevo valor de salud del jugador.
        healthPlayer.CurrentHealth.OnValueChanged += OnClientHealthChanged;

        //Evento de muerte del jugador
        healthPlayer.OnDie += Die;

        ChangeNameRpc(Utilities.GetRandomUsername());

        if (IsServer)
        {
            transform.position = spawnPointManager.GetRandomSpawnPoint();
        }

        //Inicializamos la barra de salud con el valor actual de salud del jugador 
        // al momento de la creación del objeto en la red.
        OnClientHealthChanged(healthPlayer.CurrentHealth.Value, healthPlayer.CurrentHealth.Value);
        //OnClientHealthChanged(MAX_LIFE, MAX_LIFE);

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        //health.OnValueChanged -= OnClientHealthChanged;
        username.OnValueChanged -= OnClientUsernameChanged;
        // Mediante el componente HealthPlayer, accedemos a la variable de salud y nos desuscribimos 
        // del evento OnValueChanged para evitar posibles problemas de memoria o referencias a 
        // objetos que ya no existen cuando el jugador se despawnea. Esto es importante para mantener 
        // la estabilidad y el rendimiento del juego.
        spawns.OnValueChanged -= OnSpawnsChanged;
        //ApplyDamage(0);
        // Al igual que con la suscripción al evento de cambio de salud, 
        // también nos desuscribimos del evento de muerte del jugador para asegurarnos de que no haya referencias 
        // a objetos que ya no existen cuando el jugador se despawnea.
        healthPlayer.CurrentHealth.OnValueChanged -= OnClientHealthChanged;
        //Evento de muerte del jugador
        healthPlayer.OnDie -= Die;
        //GetComponent<HealthPlayer>().TakenDamage(0);
    }

    private void OnClientUsernameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        m_UsernameLabel.text = newValue.ToString();
    }


    [Rpc(SendTo.Server)]
    public void ChangeNameRpc(FixedString128Bytes newValue)
    {
        if (!IsServer) return;
        username.Value = newValue;
    }



    void OnClientHealthChanged(int previousHealth, int newHealth)
    {
        txtHealth.text = newHealth.ToString();
    }

    void OnSpawnsChanged(int previousValue, int newValue)
    {
        txtSpawns.text = newValue.ToString();
    }


    private void Die()
    {
        if (!IsServer) return;


        Respawn();
    }

    private void Respawn()
    {
        if (!IsServer) return;
        transform.position = spawnPointManager.GetRandomSpawnPoint();
        healthPlayer.ResetHealth();
        spawns.Value++;
    }
}

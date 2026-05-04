using UnityEngine;
using Unity.Netcode;
using System;
public class HealthPlayer : NetworkBehaviour
{

    // Creamos una clase HealthPlayer que hereda de NetworkBehaviour para manejar la salud del jugador en un juego multijugador utilizando Netcode for GameObjects.
    // Esta clase contiene una variable de salud máxima (maxHealth) que se puede configurar desde 
    // el inspector de Unity, y una NetworkVariable llamada CurrentHealth que representa la salud actual 
    // del jugador y se sincroniza automáticamente entre el servidor y los clientes.
    [SerializeField] private int maxHealth = 100;

    // La variable de salud actual del jugador es una NetworkVariable de tipo int, 
    // lo que permite que su valor se sincronice automáticamente entre el servidor y los clientes.
    public int MaxHealth => maxHealth;

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead;

    public Action OnDie;




    // Solo el servidor inicializa el maximo de Vida del Player
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        CurrentHealth.Value = maxHealth;
        isDead = false;
    }

    public void TakenDamage(int damage)
    {
        if (!IsServer) return;
        if (isDead) return;
        // Creamos una nueva variable newHealth en la cual guardaremos 
        // la vida restante que quda despues de restar el actual valor de Salud - el daño recibido.
        int newHealth = CurrentHealth.Value - damage;
        Debug.Log($"Player took damage: {damage}, Health: {CurrentHealth.Value}");
        // Mediante Mathf.Clamp nos aseguramos de que al restar vida si esta fuese a dar un valor por debajo
        // de 0 está no pueda quedar en valor negativo y se asegure que sea 0
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, maxHealth);
        Debug.Log($"DAMAGE CALLED ON SERVER: {damage}");

        if (CurrentHealth.Value == 0)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }

    public void ResetHealth()
    {
        if (!IsServer) return;
        CurrentHealth.Value = maxHealth;
        isDead = false;
    }



}

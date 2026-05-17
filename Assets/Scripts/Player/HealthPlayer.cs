using UnityEngine;
using Unity.Netcode;
using System;

// Administra la salud del jugador en un entorno de red.
// El servidor es quien controla la salud y sincroniza el valor entre los clientes.
public class HealthPlayer : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100; // Salud máxima configurada en el inspector.

    public int MaxHealth => maxHealth; // Propiedad de solo lectura para acceder a la salud máxima.

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(); // Salud actual sincronizada en la red.

    private bool isDead; // Indica si el jugador ya ha muerto.

    public Action OnDie; // Evento que se dispara cuando el jugador muere.

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return; // Solo el servidor inicializa los valores de salud.

        CurrentHealth.Value = maxHealth; // Fija la salud inicial al máximo.
        isDead = false; // Reinicia el estado de muerte.
    }

    public void TakenDamage(int damage)
    {
        if (!IsServer) return; // Solo el servidor aplica daño.
        if (isDead) return; // Ignora daño si ya está muerto.

        int newHealth = CurrentHealth.Value - damage; // Calcula la nueva salud.
        Debug.Log($"Player took damage: {damage}, Health: {CurrentHealth.Value}");

        // Clamp asegura que la salud no baje de 0 ni supere el máximo.
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, maxHealth);
        Debug.Log($"DAMAGE CALLED ON SERVER: {damage}");

        if (CurrentHealth.Value == 0)
        {
            isDead = true; // Marca al jugador como muerto.
            OnDie?.Invoke(); // Dispara el evento de muerte si hay suscriptores.
        }
    }

    public void ResetHealth()
    {
        if (!IsServer) return; // Solo el servidor puede resetear la salud.
        CurrentHealth.Value = maxHealth; // Vuelve a la salud máxima.
        isDead = false; // Reinicia el estado de muerte.
    }
}

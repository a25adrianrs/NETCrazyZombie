using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

// Muestra la salud del jugador en una barra de UI y la actualiza cuando cambia la NetworkVariable.
public class HealthDisplayPlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private HealthPlayer health; // Referencia al script de salud del jugador.
    [SerializeField] private Image healthBarImage; // Imagen UI que indica el porcentaje de salud.

    public override void OnNetworkSpawn()
    {
        // Solo los clientes necesitan mostrar la UI de salud.
        if (!IsClient) return;

        // Se suscribe al evento de cambio de valor de la NetworkVariable CurrentHealth.
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, health.CurrentHealth.Value); // Actualiza la barra con el valor inicial.
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        // Se desuscribe del evento para evitar referencias a objetos destruidos.
        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int oldHealth, int currentHealth)
    {
        // Convierte la salud actual en un porcentaje para la barra de UI.
        healthBarImage.fillAmount = (float)currentHealth / health.MaxHealth;
    }
}

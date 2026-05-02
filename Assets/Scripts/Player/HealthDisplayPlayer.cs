using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
public class HealthDisplayPlayer : NetworkBehaviour
{
    // Este script se encarga de mostrar la barra de salud del jugador en el UI y actualizarla 
    // cada vez que cambie la salud del jugador.
    [Header("References")]
    [SerializeField] private HealthPlayer health; //Referencia al script de salud del jugador para acceder a su NetworkVariable de salud actual
    [SerializeField] private Image healthBarImage;//Referencia a la imagen del UI que representa la barra de salud del jugador

    public override void OnNetworkSpawn()
    {
        // Solo los clientes necesitan mostrar la salud, por lo que verificamos si no es cliente y retornamos
        if (!IsClient) return;

        // Nos suscribimos al evento OnValueChanged de la NetworkVariable CurrentHealth del script HealthPlayer para que cada vez que cambie la salud del jugador se ejecute el método HandleHealthChanged
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        // Llamamos al método HandleHealthChanged para actualizar la barra de salud con el valor inicial de salud del jugador
        HandleHealthChanged(0, health.CurrentHealth.Value);

    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        // Nos desuscribimos del evento OnValueChanged para evitar posibles errores o 
        // fugas de memoria cuando el objeto se destruya o deje de ser relevante para el cliente
        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int oldHealth, int currentHealth)
    {
        // Actualizamos la barra de salud del jugador dividiendo la salud actual entre la salud máxima 
        // para obtener un valor entre 0 y 1 que representa el porcentaje de salud restante
        healthBarImage.fillAmount = (float)currentHealth / health.MaxHealth;
    }
}

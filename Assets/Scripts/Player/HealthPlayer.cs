using UnityEngine;
using Unity.Netcode;
using System;
public class HealthPlayer : NetworkBehaviour
{

    [SerializeField] private int maxHealth = 100;

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
        // Mediante Mathf.Clamp nos aseguramos de que al restar vida si esta fuese a dar un valor por debajo
        // de 0 está no pueda quedar en valor negativo y se asegure que sea 0
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, maxHealth);

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

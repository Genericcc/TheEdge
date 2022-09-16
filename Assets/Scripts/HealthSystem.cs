using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] SquadManager squadManager;

    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    private int health = 1;
    private int healthMax;

    public void Damage()
    {
        if(squadManager.GetShields() > 0)
        {
            squadManager.DamageShields();
        }
        else
        {
            health--;
        }

        if(health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if(health == 0)
        {
            Die();
        }

        Debug.Log(health);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() 
    {
        return (float)health / healthMax;
    }
}

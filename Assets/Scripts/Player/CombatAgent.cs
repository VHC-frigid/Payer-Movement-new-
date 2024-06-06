using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAgent : MonoBehaviour
{
    //how much halth the agent currently has
    [SerializeField] protected float healthCurrent;

    //the largest amount of health allowed
    [SerializeField] protected float healthMax;

    protected virtual void Start()
    {
        healthCurrent = healthMax;
    }

    public void TakeDamage(float damage)
    {
        healthCurrent -= damage;
        if(healthCurrent <= 0)
        {
            healthCurrent = 0;
            EndOfLife();
        }
    }

    public void Heal(float heal)
    {
        healthCurrent = Mathf.Clamp(healthCurrent + heal, 0, healthMax);
    }

    //abstract method = must be filled in by child class
    protected abstract void EndOfLife();
 
}

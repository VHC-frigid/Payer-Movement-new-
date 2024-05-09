using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] protected float damage;
    protected ParticleSystem psysWeapon;
    
    private void Start()
    {
        psysWeapon = GetComponent<ParticleSystem>();
    }

    protected virtual void SetDirection()
    {
        psysWeapon.transform.forward = Vector3.up;
    }
    
    public void Shoot()
    {
        if (psysWeapon.isStopped)
        {
            SetDirection();
            psysWeapon.Play();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<CombatAgent>(out CombatAgent agent))
        {
            agent.TakeDamage(damage);
        }
    }
}

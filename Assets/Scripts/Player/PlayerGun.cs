using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : HitScanFromCamera
{
    [SerializeField] private float damage;
    [SerializeField] private float cost;

    private void Shoot()
    {
        //try to get a hit
        RaycastHit hit = CastFromSceenCentre();

        if (hit.collider)
        {

            Debug.Log($"Shot {hit.collider.gameObject.name}");
            //check the hit for a combat agent
            if (hit.collider.TryGetComponent<CombatAgent>(out CombatAgent agent))
            {
                Debug.Log($"Shot agent {agent.gameObject.name} for {damage} damage");
                //make them take damage if found
                agent.TakeDamage(damage);
            }
        }

    }

    private void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }
}

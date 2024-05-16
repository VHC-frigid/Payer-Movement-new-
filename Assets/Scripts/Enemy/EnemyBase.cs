using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scannable))]
public abstract class EnemyBase : CombatAgent
{
    [Tooltip("The minimum distance from the player before the enemy attacks")]
    [SerializeField] protected float aggroRange;

    protected Transform playerTransform;

    //is th enemy currently attacking?
    protected bool isAttacking;

    protected EnemyGun myGun;

    protected override void Start()
    {
        playerTransform = FindObjectOfType<CustomController>().transform;
        myGun =  GetComponentInChildren<EnemyGun>();
    }

    protected virtual void Update()
    {
        isAttacking = false;
        Debug.Log(Vector3.Distance(playerTransform.position, transform.position).ToString());
        if (Vector3.Distance(playerTransform.position,transform.position) < aggroRange)
        {
            DoAttack();
        }
    }

    protected virtual void DoAttack()
    {
        if (myGun)
        {
            isAttacking = true;
            myGun.Shoot();
        }
    }
    
    protected override void EndOfLife()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scannable))]
public abstract class EnemyBase : CombatAgent
{
    [System.Serializable]
    public struct DropTableEntry
    {
        //how likely the itm will drop (0-100)
        public float chance;
        //how many should drop
        public int count;
        //what type should drop
        public Item.Type type;
    }

    [SerializeField] private DropTableEntry[] dropTable;

    [Tooltip("The minimum distance from the player before the enemy attacks")]
    [SerializeField] protected float aggroRange;



    protected Transform playerTransform;

    //is th enemy currently attacking?
    protected bool isAttacking;

    protected EnemyGun myGun;

    protected override void Start()
    {
        base.Start();
        playerTransform = FindObjectOfType<CustomController>().transform;
        myGun =  GetComponentInChildren<EnemyGun>();
    }

    protected virtual void Update()
    {
        isAttacking = false;
        float range = aggroRange;
        if (Input.GetButton("Crouch"))
        {
            range *= 0.5f;
        }
        //Debug.Log(Vector3.Distance(playerTransform.position, transform.position).ToString());
        if (Vector3.Distance(playerTransform.position,transform.position) < range)
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
        //roll a random number
        float roll = Random.Range(0f, 100f);

        foreach (DropTableEntry drop in dropTable)
        {
            if (roll < drop.chance)
            {
                //first, initialise a counting variable (int i)
                //second, define the loop condition (if this is true, keep looping)
                //third, define what should happen at the end of a loop
                for (int i = 0; i < drop.count; i++)
                {
                    Item.Spawn(drop.type, transform.position);
                    //Debug.Log($"Dropped item: {drop.type}");
                }
            }
        }
        
        gameObject.SetActive(false);
    }
}

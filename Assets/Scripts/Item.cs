using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Scannable
{
    public enum Type
    {
        Heal,
        //1-9 are reserved for future items
        UpgradeHealth = 10,
        UpgradeStamina = 20,
        UpgradeGrapple = 30,
        UpgradeGun = 40
    }

    private Type type;
    private bool magnetised;
    private Vector3 magnetisedPosition;
    private float magnetisedStart;

    private static Transform _playerTransform;
    private static Transform playerTransform
    {
        get
        {
            if (_playerTransform == null)
                _playerTransform = FindObjectOfType<CustomController>().transform;
            return _playerTransform;
        }
    }

    private static GameObject _prefab;

    private static GameObject prefab
    {
        get
        {
            if (_prefab == null)
            {
                _prefab = Resources.Load("Prefabs/Item") as GameObject;
            }        
            return _prefab;
        }
    }

    // => is lambda expressio, it evaluates left and applies the value to right 
    private float distanceToPlayer => Vector3.Distance(transform.position, playerTransform.position);

    public Type ItemType => type;

    public static Item Spawn(Type type, Vector3 position)
    {
        Item spawnedItem = Instantiate(prefab, position, Quaternion.identity).GetComponent<Item>();
        spawnedItem.type = type;

        switch (type)
        {
            case Type.Heal:
                spawnedItem.GetComponent<Renderer>().material.color = Color.red;
                break;
            case Type.UpgradeHealth:
                spawnedItem.scanName = "Monster energy";
                spawnedItem.scanDescription = "MAXIUM ENERGY";
                spawnedItem.transform.localScale = Vector3.one * 0.5f;
                spawnedItem.GetComponent<Renderer>().material.color = Color.green;
                break;
            case Type.UpgradeGun:
                break;
            case Type.UpgradeStamina:
                break;
            case Type.UpgradeGrapple:
                break;
        }
        return spawnedItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (!magnetised)
        {
            return;
        }
        transform.position = Vector3.Lerp(magnetisedPosition, playerTransform.position, Time.time - magnetisedStart * 3);
    }

    private void FixedUpdate()
    {
        if (magnetised)
        {
            return;
        }

        if (distanceToPlayer < 2f)
        {
            magnetised = true;
            magnetisedPosition = transform.position;
            magnetisedStart = Time.time;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scannable : MonoBehaviour
{
    
    public enum Category
    {
        Enviroment,
        Enemy,
        Item
    }

    [SerializeField] private Category scanCategory;
    [SerializeField] protected string scanName;
    [SerializeField] protected string scanDescription;

    //A property allows us to GET a copy of information without letting us SET it
    public string ScanName
    {
        get
        {
            return scanName;
        }
    }

    public string ScanDescription
    {
        get
        {
            return scanDescription;
        }
    }

    public Category ScanCategory
    {
        get
        {
            return scanCategory;
        }
    }

    private static Scanpopup scanPopup;

    private void Start()
    {
        if (scanPopup == null)
        {
            scanPopup = FindObjectOfType<Scanpopup>();
        }
    }

    public void Scan()
    {
        FindObjectOfType<Scanpopup>().DisplayScan(scanName, scanDescription);
        Catalog.CheckNewScan(this);
    }
}

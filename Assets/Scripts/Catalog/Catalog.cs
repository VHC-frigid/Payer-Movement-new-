using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Catalog
{
    // < > is where we put what data we want, e.g. <MyClassHere> 
    private static List<Scannable> scannedItems = new List<Scannable>();

    public static List<Scannable> ScannedItems
    {
        get
        {
            return new List<Scannable>(scannedItems);
        }
    }


    public static void CheckNewScan(Scannable scan)
    {
        foreach (Scannable item in scannedItems)
        {
            if (scan.ScanName == item.ScanName)
            {
                return;
            }
        }
        AddNewScan(scan);
    }

    private static void AddNewScan(Scannable scan)
    {
        scannedItems.Add(scan);
        Debug.Log($"Found new scan: {scan.ScanName}");
    }
}

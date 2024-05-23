using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CataloueScreen : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject catalogueItemPrefab;
    [SerializeField] private GameObject cataloguePanel;

    public void CloseScreen()
    {
        cataloguePanel.SetActive(false);
    }

    public void UpdateCatalogueUI()
    {
        ClearCurrentUI();
        List<Scannable> list = Catalog.ScannedItems;
        AddItemFromCatalogue(list);
    }

    private void ClearCurrentUI()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void AddItemFromCatalogue(List<Scannable> catalogue)
    {
        foreach (Scannable item in catalogue)
        {
            AddItem(item);
        }
    }

    private void AddItem(Scannable item)
    {
        //spawn in one of our item panels
        GameObject panel = Instantiate(catalogueItemPrefab, content);
        //get the text components from the panel
        Text[] panelText = panel.GetComponentsInChildren<Text>();
        //update each component to show name/description
        panelText[0].text = item.ScanName;
        panelText[1].text = item.ScanDescription;
    }
}

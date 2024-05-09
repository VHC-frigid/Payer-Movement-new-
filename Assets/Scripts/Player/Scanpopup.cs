using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scanpopup : MonoBehaviour
{
    //
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text nameLabel;
    [SerializeField] private Text descriptionLabel;
    
    //
    [SerializeField] private float displayDuration;

    //
    private float displayBegin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if the duration of the popup has expired
        if (Time.time - displayBegin > displayDuration)
        {
            popupPanel.SetActive(false);
        }
    }

    public void DisplayScan(string name, string description)
    {
        //enable the popup panel
        popupPanel.SetActive(true);
        //update the text
        nameLabel.text = name;
        descriptionLabel.text = description;
        //set the time this popup began
        displayBegin = Time.time;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{

    [SerializeField] private Image healthBar;
    [SerializeField] private RectTransform healthBarBG;
    [SerializeField] private Image staminaBar;

    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Color staminaColor;

    // Start is called before the first frame update
    void Start()
    {
        //Image.fillamount is between 0 and 1
        healthBar.fillAmount = 1;
        staminaBar.fillAmount = 1;

        staminaBar.color = staminaColor;

        //Gradient.Evaluate() takes a number between 0 and 1
        healthBar.color = healthGradient.Evaluate(1);
    }

    public void UpdateHUD(float healthPercent, float staminaPercent)
    {
        healthBar.fillAmount = healthPercent;
        staminaBar.fillAmount = staminaPercent;
        healthBar.color = healthGradient.Evaluate(healthPercent);
    }

    public void SetNewSize(float healthMax)
    {
        healthBarBG.sizeDelta = new(healthMax * 25f, healthBarBG.sizeDelta.y);
    }

}

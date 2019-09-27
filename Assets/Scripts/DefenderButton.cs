using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenderButton : MonoBehaviour
{
    // Variables
    [Header("Properties")]
    public Image image;
    public int goldCost;
    public int troopCost;

    [Header("Component References")]
    public TextMeshProUGUI goldCostText;
    public TextMeshProUGUI troopCostText;
    public GameObject defenderPrefab;


    // Setup
    private void Awake()
    {
        SetGoldCostTextValue(goldCost);
        SetTroopCostTextValue(troopCost);
    }

    // Properties    
    public Sprite Sprite
    {
        get
        {
            return image.sprite;
        }        
    }    

    // Text + Visuals related methods

    public void SetGoldCostTextValue(int newValue)
    {
        goldCostText.text = newValue.ToString();
    }
    public void SetTroopCostTextValue(int newValue)
    {
        troopCostText.text = newValue.ToString();
    }

    // Input
    public void OnDefenderButtonClicked()
    {
        DefenderPanelManager.Instance.PickDefender(this);
    }

}

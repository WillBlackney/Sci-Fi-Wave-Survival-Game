using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderPanelManager : Singleton<DefenderPanelManager>
{
    public DefenderButton ClickedDefender { get; set; }
    private void Update()
    {
        HandleEscape();
    }
    public void PickDefender(DefenderButton defenderButton)
    {
        // If player does not have enough gold
        if (PlayerDataManager.Instance.currentGold < defenderButton.GoldPrice)
        {
            Debug.Log("Player does not have enough gold to purchase " + defenderButton.DefenderPrefab.name);
            return;
        }
        // If player does not have availble troop points
        if (PlayerDataManager.Instance.currentTroopCount + defenderButton.troopCost > PlayerDataManager.Instance.currentMaxTroopCount)
        {
            Debug.Log("Can't recruit troop: recruting troop would bring player over the maximum troop count" + defenderButton.DefenderPrefab.name);
            return;
        }

        Debug.Log("Defender button for " + defenderButton.name + " clicked on");
        ClickedDefender = defenderButton;
        Hover.Instance.Activate(defenderButton.defenderPrefab.GetComponent<SpriteRenderer>().sprite);
    }

    // This method is called from TileScripts
    public void BuyDefender()
    {
        if (PlayerDataManager.Instance.currentGold >= ClickedDefender.GoldPrice)
        {
            PlayerDataManager.Instance.ModifyGold(-ClickedDefender.GoldPrice);            
            Hover.Instance.Deactivate();
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || 
            Input.GetKeyDown(KeyCode.Mouse1))
        {
            Hover.Instance.Deactivate();
        }
    }    

    
}

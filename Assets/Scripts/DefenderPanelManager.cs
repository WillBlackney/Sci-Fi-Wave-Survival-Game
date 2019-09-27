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
        // if it is the enemies turn, prevent placement
        if (!TurnManager.Instance.currentlyPlayersTurn)
        {
            Debug.Log("Player cannot place new defenders during enemy turn...");
        }

        // If player does not have enough gold
        if (PlayerDataManager.Instance.currentGold < defenderButton.goldCost)
        {
            Debug.Log("Player does not have enough gold to purchase " + defenderButton.defenderPrefab.name);
            return;
        }
        // If player does not have availble troop points
        if (PlayerDataManager.Instance.currentTroopCount + defenderButton.troopCost > PlayerDataManager.Instance.currentMaxTroopCount)
        {
            Debug.Log("Can't recruit troop: recruting troop would bring player over the maximum troop count" + defenderButton.defenderPrefab.name);
            return;
        }

        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetSpaceShipControlZoneTile());
        Debug.Log("Defender button for " + defenderButton.name + " clicked on");
        ClickedDefender = defenderButton;
        Hover.Instance.Activate(defenderButton.Sprite);
    }

    // This method is called from TileScripts
    public void BuyDefender()
    {
        LevelManager.Instance.UnhighlightAllTiles();
        PlayerDataManager.Instance.ModifyGold(-ClickedDefender.goldCost);
        Hover.Instance.Deactivate();
        /*
        if (PlayerDataManager.Instance.currentGold >= ClickedDefender.goldCost)
        {
            LevelManager.Instance.UnhighlightAllTiles();
            PlayerDataManager.Instance.ModifyGold(-ClickedDefender.goldCost);            
            Hover.Instance.Deactivate();
        }
        */
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || 
            Input.GetKeyDown(KeyCode.Mouse1))
        {
            LevelManager.Instance.UnhighlightAllTiles();
            Hover.Instance.Deactivate();
        }
    }    

    
}

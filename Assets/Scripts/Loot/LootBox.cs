    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootBox : WorldObject
{
    [Header("Loot Box Specific Component References")]
    public Animator myAnim;    
    public GameObject myVisualParent;
    public SpriteRenderer mySpriteRenderer;
    public TextMeshProUGUI countDownText;

    [Header("Loot Box Specific Properties")]
    public Color normalColor;
    public Color highlightColor;
    public bool opened;
    public int countDown;

    // Conditional checks + bools
    #region
    public bool HasDefenderInRange()
    {
        return true;
        bool boolReturned = false;
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, myTile);

        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (adjacentTiles.Contains(defender.TileCurrentlyOn))
            {
                boolReturned = true;
                break;
            }
        }

        return boolReturned;
    }
    public bool CanBeOpened()
    {
        if (HasDefenderInRange() && TurnManager.Instance.currentlyPlayersTurn && opened == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
#endregion

    // Mouse Events + Input
    #region
    public void OnMouseDown()
    {
        OnLootBoxClicked();
    }
    public void OnMouseEnter()
    {
        mySpriteRenderer.color = highlightColor;
    }
    public void OnMouseExit()
    {
        mySpriteRenderer.color = normalColor;
    }
    public void OnLootBoxClicked()
    {
        Debug.Log("Loot box click detected...");
        if (CanBeOpened())
        {
            opened = true;
            LootBoxManager.Instance.StartNewLootScreenEvent();
            LootBoxManager.Instance.DestroyLootBox(this);
        }
    }
    #endregion

    // Visual + related
    public void ModifyCountDown(int countDownGainedOrLost)
    {
        countDown += countDownGainedOrLost;
        countDownText.text = countDown.ToString();        
    }
}

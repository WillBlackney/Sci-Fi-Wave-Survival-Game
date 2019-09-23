using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TowerButton ClickedButton { get; set; }

    private int currentGold;

    [SerializeField] private Text goldText;

    public int CurrentGold
    {
        get
        {
            return currentGold;
        }

        set
        {
            currentGold = value;
            //this.goldText.text = value.ToString() + "<color=lime>$</color>";
        }
    }

    private void Start()
    {
        CurrentGold = 100;
    }

    private void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerButton towerButton)
    {
        Debug.Log("Defender button for " + towerButton.name + " clicked on");
        ClickedButton = towerButton;
        Hover.Instance.Activate(towerButton.defenderPrefab.GetComponent<SpriteRenderer>().sprite);
        /*
        if(CurrentGold >= towerButton.GoldPrice)
        {
            ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
        }
        */

    }

    // This method is called from TileScripts
    public void BuyTower()
    {
        if (CurrentGold >= ClickedButton.GoldPrice)
        {
            CurrentGold -= ClickedButton.GoldPrice;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Header("Component References")]
    public TextMeshProUGUI currentGoldText;
    public TextMeshProUGUI currentTroopCountText;
    public TextMeshProUGUI currentMaxTroopCountText;

    [Header("Properties")]    
    public int currentGold;
    public int currentTroopCount;
    public int currentMaxTroopCount;

    private void Start()
    {
        InitializeSetup();
    }
    public void InitializeSetup()
    {
        ModifyGold(GlobalSettings.Instance.startingGold);
        ModifyCurrentMaxTroopCount(GlobalSettings.Instance.startingMaxTroopCount);
        ModifyCurrentTroopCount(0);
    }
    public void ModifyGold(int goldGainedOrLost)
    {
        currentGold += goldGainedOrLost;
        currentGoldText.text = currentGold.ToString();
    }

    public void ModifyCurrentTroopCount(int troopCountGainedOrLost)
    {
        currentTroopCount += troopCountGainedOrLost;
        UpdateCurrentTroopCountText(currentTroopCount);
    }
    public void UpdateCurrentTroopCountText(int newValue)
    {
        currentTroopCountText.text = newValue.ToString();
    }
    public void ModifyCurrentMaxTroopCount(int troopCountGainedOrLost)
    {
        currentMaxTroopCount += troopCountGainedOrLost;
        UpdateCurrentMaxTroopCountText(currentMaxTroopCount);
    }
    public void UpdateCurrentMaxTroopCountText(int newValue)
    {
        currentMaxTroopCountText.text = newValue.ToString();
    }

    public void GenerateIncomeOnPlayerTurnStart()
    {
        ModifyGold(GlobalSettings.Instance.passiveGoldIncome);
    }
}

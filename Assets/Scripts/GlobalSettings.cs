using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : Singleton<GlobalSettings>
{
    [Header("Player Properties")]
    public int startingGold;
    public int startingMaxTroopCount;
    public int passiveGoldIncome;
}

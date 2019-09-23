using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] public GameObject defenderPrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int goldPrice;
    [SerializeField] private Text priceText;
       

    public GameObject TowerPrefab
    {
        get
        {
            return defenderPrefab;
        }       
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }        
    }

    public int GoldPrice
    {
        get
        {
            return goldPrice;
        }        
    }
}

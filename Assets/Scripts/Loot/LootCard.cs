using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [Header("Component References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image rewardImage;
    public Image parentImage;

    [Header("Properties")]
    public LootDataSO myLootData;
    public Color normalColor;
    public Color highlightColor;

    // Setup + Initialization
    #region
    public void RunSetupFromLootDataSO(LootDataSO data)
    {
        myLootData = data;
        nameText.text = data.lootName;
        descriptionText.text = data.lootDescription;
        rewardImage.sprite = data.lootImage;
    }
    #endregion

    // Mouse + input related
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        parentImage.color = highlightColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        LootBoxManager.Instance.lootRewardScreenVisualParent.SetActive(false);
        LootBoxManager.Instance.RewardLootFromLootCard(myLootData);
        LootBoxManager.Instance.FadeOutLootScreen();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        parentImage.color = normalColor;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI apCostText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI coolDownText;
    public TextMeshProUGUI descriptionText;
    public Image myImage;

    public void InitializeSetup(AbilityDataSO abilityData)
    {
        nameText.text = abilityData.abilityName;
        apCostText.text = abilityData.abilityAPCost.ToString();
        rangeText.text = abilityData.abilityRange.ToString();
        coolDownText.text = abilityData.abilityBaseCooldownTime.ToString();
        myImage.sprite = abilityData.abilityImage;
    }

    public void SetInfoPanelVisbility(bool onOrOff)
    {
        infoPanel.SetActive(onOrOff);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetInfoPanelVisbility(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetInfoPanelVisbility(false);
    }

}

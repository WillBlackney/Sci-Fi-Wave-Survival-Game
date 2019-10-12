using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public GameObject infoPanel;
    public Sprite statusImage;
    public TextMeshProUGUI statusStacksText;
    public TextMeshProUGUI statusNameText;
    public TextMeshProUGUI statusDescriptionText;   

    [Header ("Properties")]
    public string statusName;
    public string statusDescription;
    public int statusStacks;    

    public void InitializeSetup(StatusIcon iconData)
    {
        statusImage = iconData.statusImage;
        GetComponent<Image>().sprite = statusImage;

        statusName = iconData.statusName;
        statusNameText.text = statusName;
        statusDescription = iconData.statusDescription;
        statusDescriptionText.text = statusDescription;
        statusStacks = iconData.statusStacks;
        statusStacksText.text = statusStacks.ToString();
    }

    public void ModifyStatusIconStacks(int stacksGainedOrLost)
    {
        statusStacks += stacksGainedOrLost;
        statusStacksText.text = statusStacks.ToString();
        if(statusStacks == 0)
        {
            statusStacksText.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //SpellInfoBox.Instance.ShowInfoBox(statusName, 0, 0, 0, statusDescription);
        SetInfoPanelVisibility(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //SpellInfoBox.Instance.HideInfoBox();
        SetInfoPanelVisibility(false);
    }

    public void SetInfoPanelVisibility(bool onOrOff)
    {
        infoPanel.SetActive(onOrOff);
    }
}

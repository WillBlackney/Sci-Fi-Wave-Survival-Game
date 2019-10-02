using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text + Component References ")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI minDamageText;
    public TextMeshProUGUI maxDamageText;
    public GameObject myInfoPanel;
    public Image myImage;
    public CanvasGroup myCanvasGroup;
    public WeaponDataSO myWeaponData;

    [Header("Properties")]
    public bool fadingIn;

    // Initialization / Setup
    public void SetWeapon(WeaponDataSO data)
    {
        Debug.Log("SetWeapon() called....");
        myWeaponData = data;
        nameText.text = data.weaponName;
        rangeText.text = data.weaponRange.ToString();
        minDamageText.text = data.weaponMinDamage.ToString();
        maxDamageText.text = data.weaponMaxDamage.ToString();
        myImage.sprite = data.weaponImage;
    }


    // Pointer and Input Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetInfoPanelVisibility(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetInfoPanelVisibility(false);
    }

    public void SetInfoPanelVisibility(bool onOrOff)
    {
        myInfoPanel.SetActive(onOrOff);
        if (onOrOff == true)
        {
            FadeInPanel();
        }
        else
        {
            fadingIn = false;
            myCanvasGroup.alpha = 0;
        }
        
    }

    public void FadeInPanel()
    {
        StartCoroutine(FadeInPanelCoroutine());
    }
    public IEnumerator FadeInPanelCoroutine()
    {
        fadingIn = true;
        while (myCanvasGroup.alpha < 1 && fadingIn == true)
        {
            myCanvasGroup.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    public Enemy myEnemy;
    public TextMeshProUGUI nameText;
    public Image myCharacterImage;
    public CanvasGroup myCanvasGroup;
    public GameObject panelParent;
    public GameObject abilityTabParent;
    public GameObject abilitiesParent;
    public GameObject descriptionParent;
    public WeaponSlot rangedWeaponTab;
    public WeaponSlot meleeWeaponTab;
    
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI mobilityText;
    public TextMeshProUGUI aimText;
    public TextMeshProUGUI defenseText;

    public void InitializeSetup(Enemy enemyParent)
    {
        myEnemy = enemyParent;
        nameText.text = myEnemy.myName;
        myCharacterImage.sprite = myEnemy.mySpriteRenderer.sprite;
        
        energyText.text = myEnemy.baseEnergy.ToString();
        mobilityText.text = myEnemy.baseMobility.ToString();
        aimText.text = myEnemy.baseAim.ToString();
        defenseText.text = myEnemy.baseDefense.ToString();

    }
    public void SetPanelViewState(bool onOrOff)
    {        
        if(onOrOff == true)
        {
            StartCoroutine(EnablePanelView());
            BlackScreenManager.Instance.FadeOut(8, 0.5f, true);
        }
        else
        {
            StartCoroutine(DisablePanelView());
            BlackScreenManager.Instance.FadeIn(8);
        }
    }
    public IEnumerator EnablePanelView()
    {
        myCanvasGroup.alpha = 0;
        panelParent.SetActive(true);
        while (myCanvasGroup.alpha < 1)
        {
            myCanvasGroup.alpha += 0.02f * 10;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator DisablePanelView()
    {
        myCanvasGroup.alpha = 1;
        panelParent.SetActive(true);
        while (myCanvasGroup.alpha < 1)
        {
            myCanvasGroup.alpha -= 0.02f * 10;
            yield return new WaitForEndOfFrame();
        }
        panelParent.SetActive(false);
    }
    public void OnDescriptionButtonClicked()
    {
        abilitiesParent.SetActive(false);
        descriptionParent.SetActive(true);
    }
    public void OnAbilitiesButtonClicked()
    {
        abilitiesParent.SetActive(true);
        descriptionParent.SetActive(false);
    }
    public void AddAbilityToolTipToView(AbilityDataSO ability)
    {
        GameObject newAbilityTabGO = Instantiate(PrefabHolder.Instance.abilityTabPrefab, abilityTabParent.transform);
        newAbilityTabGO.GetComponent<AbilityTab>().InitializeSetup(ability);
    }
    public void AddAbilityToolTipToView(StatusIcon ability)
    {
        GameObject newAbilityTabGO = Instantiate(PrefabHolder.Instance.abilityTabPrefab, abilityTabParent.transform);
        newAbilityTabGO.GetComponent<AbilityTab>().InitializeSetup(ability);
    }
    public void SetWeaponTab(WeaponSlot slot, WeaponDataSO weapon)
    {
        slot.gameObject.SetActive(true);
        slot.SetWeapon(weapon);
    }
}

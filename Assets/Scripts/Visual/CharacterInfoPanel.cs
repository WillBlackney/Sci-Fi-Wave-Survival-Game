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
    public GameObject panelParent;
    public GameObject abilityTabParent;
    public GameObject abilitiesParent;
    public GameObject descriptionParent;
    
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
        panelParent.SetActive(onOrOff);
    }
    public void EnablePanelView()
    {
        panelParent.SetActive(true);
    }

    public void DisablePanelView()
    {
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
}

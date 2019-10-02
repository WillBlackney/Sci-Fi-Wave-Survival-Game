using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitChanceHover : Singleton<HitChanceHover>
{
    [Header("Component Reference")]
    public GameObject visualParent;
    public TextMeshProUGUI hitChanceText;
    public CanvasGroup myCanvasGroup;

    [Header("Modifier Object References")]
    public GameObject targetDefense;
    public TextMeshProUGUI targetDefenseText;
    public GameObject attackerAim;
    public TextMeshProUGUI attackerAimText;
    public GameObject attackerEntrenched;
    public GameObject attackerDeadEye;
    public GameObject targetHalfCover;
    public GameObject targetFullCover;

    [Header("Properties")]
    public bool fadingIn;


    // Visual + visibility + location related
    #region
    public void SetVisibility(bool onOrOff)
    {
        visualParent.SetActive(onOrOff);
        if(onOrOff == true)
        {
            FadeIn();
        }

        else if(onOrOff == false)
        {
            fadingIn = false;
            myCanvasGroup.alpha = 0;
            DisableAllModifierObjects();
        }
    }  
    public void DisableAllModifierObjects()
    {
        targetDefense.SetActive(false);
        targetHalfCover.SetActive(false);
        targetFullCover.SetActive(false);
        attackerAim.SetActive(false);
        attackerEntrenched.SetActive(false);
        attackerDeadEye.SetActive(false);
    }
    public void MoveToEntityPosition(LivingEntity entity)
    {
        Vector3 tileWorldPos = entity.TileCurrentlyOn.WorldPosition;
        Vector3 idealWorldPos = new Vector3(tileWorldPos.x + 1, tileWorldPos.y, tileWorldPos.z);
        Vector3 finalPos = CameraManager.Instance.mainCamera.WorldToScreenPoint((tileWorldPos + idealWorldPos) / 2);
        visualParent.transform.position = finalPos;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
    public IEnumerator FadeInCoroutine()
    {
        fadingIn = true;
        while (myCanvasGroup.alpha < 1 && fadingIn == true)
        {
            myCanvasGroup.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
    // Logic + Events
    #region
    public void StartNewShowHitChanceEvent(LivingEntity target, LivingEntity attacker = null, Ability abilityUsed = null, WeaponDataSO weaponUsed = null)
    {
        // Disable modifiers from previous target
        DisableAllModifierObjects();

        // Enable panel view
        SetVisibility(true);

        // Move panel to the right of the target
        MoveToEntityPosition(target);

        // Calculate and set hit chance text
        hitChanceText.text = CombatLogic.Instance.CalculateHitChance(attacker, target).ToString();

        // Get target data and set modifier views
        EnableAndSetTargetDefenseModifier(target);
        if (PositionLogic.Instance.IsTargetInFullCover(attacker, target))
        {
            EnableTargetFullCoverModifier();
        }
        else if (PositionLogic.Instance.IsTargetInHalfCover(attacker, target))
        {
            EnableTargetHalfCoverModifier();
        }

        // Get attacker data and set modifier views
        EnableAndSetAttackerAimModifier(attacker);
        EnableAndSetAttackerEntrenchedModifier(attacker);
        EnableAndSetAttackerDeadEyeModifier(attacker);

    }
    #endregion
    // Modifier Object setters
    #region
    public void EnableAndSetTargetDefenseModifier(LivingEntity target)
    {
        targetDefense.SetActive(true);
        targetDefenseText.text = "-" + target.currentDefense.ToString();
    }
    public void EnableAndSetAttackerAimModifier(LivingEntity attacker)
    {
        attackerAim.SetActive(true);
        attackerAimText.text = "+" + attacker.currentAim.ToString();
    }
    public void EnableAndSetAttackerEntrenchedModifier(LivingEntity attacker)
    {
        if (attacker.myPassiveManager.Entrenched)
        {
            attackerEntrenched.SetActive(true);            
        }
        
    }
    public void EnableAndSetAttackerDeadEyeModifier(LivingEntity attacker)
    {
        if (attacker.myPassiveManager.DeadEye)
        {
            attackerDeadEye.SetActive(true);
        }

    }
    public void EnableTargetHalfCoverModifier()
    {
        targetHalfCover.SetActive(true);
    }
    public void EnableTargetFullCoverModifier()
    {
        targetFullCover.SetActive(true);
    }
    #endregion


}

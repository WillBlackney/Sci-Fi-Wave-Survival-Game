using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveCharger : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        WeaponLogic.Instance.AssignWeaponToEntity(this, WeaponLibrary.Instance.GetWeaponByName("Bone Claws"));

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Charge");

        myPassiveManager.LearnRegeneration(2, true);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        ActionStart:

        SetTargetDefender(GetClosestDefender());

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Charge
        else if (IsAbilityOffCooldown(charge.abilityCurrentCooldownTime) &&
            IsTargetInRange(myCurrentTarget, currentMobility) &&
            HasEnoughAP(currentAP, charge.abilityAPCost) &&
            IsAbleToMove()
            )
        {
            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);

            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Charge", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformCharge(this, myCurrentTarget, destination);
            // wait for charge event to finish
            yield return new WaitUntil(() => action.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Strike
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) && HasEnoughAP(currentAP, strike.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) == false && IsAbleToMove() && HasEnoughAP(currentAP, move.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);
            Action action = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
}

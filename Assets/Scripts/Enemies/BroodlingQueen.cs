using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodlingQueen : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        WeaponLogic.Instance.AssignWeaponToEntity(this, WeaponLibrary.Instance.GetWeaponByName("Bone Nails"));

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Lay Broodling Egg");

    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability layBroodlingEgg = mySpellBook.GetAbilityByName("Lay Broodling Egg");

        ActionStart:

        SetTargetDefender(GetClosestDefender());

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }
        // Lay Broodling Egg
        else if (HasEnoughAP(currentAP, layBroodlingEgg.abilityAPCost) && IsAbilityOffCooldown(layBroodlingEgg.abilityCurrentCooldownTime))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Lay Broodling Egg", false));
            yield return new WaitForSeconds(0.5f);

            Action layEgg = AbilityLogic.Instance.PerformLayBroodlingEgg(this);
            yield return new WaitUntil(() => layEgg.actionResolved == true);

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

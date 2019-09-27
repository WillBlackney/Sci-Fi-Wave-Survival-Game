using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodlingSpitter : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        WeaponLogic.Instance.AssignWeaponToEntity(this, WeaponLibrary.Instance.GetWeaponByName("Bone Nails"));       

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Acid Spit");
        mySpellBook.EnemyLearnAbility("Move");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability acidSpit = mySpellBook.GetAbilityByName("Acid Spit");

        ActionStart:

        SetTargetDefender(GetClosestDefender());

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
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

        // Acid Spit
        else if (IsTargetInRange(GetClosestDefender(), acidSpit.abilityRange) &&
            HasEnoughAP(currentAP, acidSpit.abilityAPCost) &&
            PositionLogic.Instance.IsThereLosFromAtoB(TileCurrentlyOn, GetClosestDefender().TileCurrentlyOn) &&
            IsAbilityOffCooldown(acidSpit.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Acid Spit", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformAcidSpit(this, myCurrentTarget.TileCurrentlyOn);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if ( (IsTargetInRange(myCurrentTarget, acidSpit.abilityRange) == false || 
            PositionLogic.Instance.IsThereLosFromAtoB(TileCurrentlyOn,myCurrentTarget.TileCurrentlyOn) == false) && 
            IsAbleToMove() && 
            HasEnoughAP(currentAP, move.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, acidSpit.abilityRange, currentMobility);
            Action action = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
}

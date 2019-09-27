using System.Collections;
using UnityEngine;

public class BroodlingGunner : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        WeaponLogic.Instance.AssignWeaponToEntity(this, WeaponLibrary.Instance.GetWeaponByName("Bone Nails"));
        WeaponLogic.Instance.AssignWeaponToEntity(this, WeaponLibrary.Instance.GetWeaponByName("Spike Rifle"));

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Shoot");
        mySpellBook.EnemyLearnAbility("Move");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");

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

        // Shoot
        else if (IsTargetInRange(GetClosestDefender(),myRangedWeapon.weaponRange) &&
            HasEnoughAP(currentAP, shoot.abilityAPCost) &&
            PositionLogic.Instance.IsThereLosFromAtoB(TileCurrentlyOn, GetClosestDefender().TileCurrentlyOn) &&
            IsAbilityOffCooldown(shoot.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(myCurrentTarget, myRangedWeapon.weaponRange) == false && IsAbleToMove() && HasEnoughAP(currentAP, move.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, myRangedWeapon.weaponRange, currentMobility);
            Action action = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
}

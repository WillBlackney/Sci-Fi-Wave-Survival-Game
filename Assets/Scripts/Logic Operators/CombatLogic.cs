using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatLogic : MonoBehaviour
{
    public static CombatLogic Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateAoEAttackEvent(LivingEntity attacker, Ability abilityUsed, TileScript aoeCentrePoint, int aoeSize, bool excludeCentreTile, bool friendlyFire)
    {
        Debug.Log("Starting new AoE damage event...");

        Defender defender = attacker.GetComponent<Defender>();
        Enemy enemy = attacker.GetComponent<Enemy>();

        List<TileScript> tilesInAoERadius = LevelManager.Instance.GetTilesWithinRange(aoeSize, aoeCentrePoint, excludeCentreTile);
        List<LivingEntity> livingEntitiesWithinAoeRadius = new List<LivingEntity>();
        List<LivingEntity> finalList = new List<LivingEntity>();

        // Get all living entities within the blast radius
        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInAoERadius.Contains(livingEntity.TileCurrentlyOn))
            {
                livingEntitiesWithinAoeRadius.Add(livingEntity);
            }
        }

        // if this ability doesn't not damage friendly targets, filter out friendly targets from the list
        if (friendlyFire == false)
        {
            foreach (LivingEntity livingEntity in livingEntitiesWithinAoeRadius)
            {
                if (defender && livingEntity.GetComponent<Enemy>())
                {
                    finalList.Add(livingEntity);
                }

                else if (enemy && livingEntity.GetComponent<Defender>())
                {
                    finalList.Add(livingEntity);
                }
            }
        }

        // Else if this ability can damage everything, add all living entities within the blast radius to the final list
        else if (friendlyFire == true)
        {
            finalList.AddRange(livingEntitiesWithinAoeRadius);
        }

        // Deal damage to all characters in the final list
        foreach (LivingEntity livingEntity in finalList)
        {
            //livingEntity.HandleDamage(livingEntity.CalculateDamage(abilityUsed.abilityPrimaryValue, livingEntity, attacker), attacker, true);
            HandleDamage(CalculateDamage(abilityUsed.abilityPrimaryValue, livingEntity, attacker, abilityUsed.abilityDamageType), attacker, livingEntity, true);
        }
    }

    // Overload method used for aoe damage events not caused by spells/abilities (e.g. volatile passive)
    public void CreateAoEAttackEvent(LivingEntity attacker, int damage, TileScript aoeCentrePoint, int aoeSize, bool excludeCentreTile, bool friendlyFire, AbilityDataSO.DamageType damageType)
    {
        Debug.Log("Starting new AoE damage event...");

        Defender defender = attacker.GetComponent<Defender>();
        Enemy enemy = attacker.GetComponent<Enemy>();

        List<TileScript> tilesInAoERadius = LevelManager.Instance.GetTilesWithinRange(aoeSize, aoeCentrePoint, excludeCentreTile);
        List<LivingEntity> livingEntitiesWithinAoeRadius = new List<LivingEntity>();
        List<LivingEntity> finalList = new List<LivingEntity>();

        // Get all living entities within the blast radius
        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInAoERadius.Contains(livingEntity.TileCurrentlyOn))
            {
                livingEntitiesWithinAoeRadius.Add(livingEntity);
            }
        }

        // if this ability doesn't not damage friendly targets, filter out friendly targets from the list
        if (friendlyFire == false)
        {
            foreach (LivingEntity livingEntity in livingEntitiesWithinAoeRadius)
            {
                if (defender && livingEntity.GetComponent<Enemy>())
                {
                    finalList.Add(livingEntity);
                }

                else if (enemy && livingEntity.GetComponent<Defender>())
                {
                    finalList.Add(livingEntity);
                }
            }
        }

        // Else if this ability can damage everything, add all living entities within the blast radius to the final list
        else if (friendlyFire == true)
        {
            finalList.AddRange(livingEntitiesWithinAoeRadius);
        }

        // Deal damage to all characters in the final list
        foreach (LivingEntity livingEntity in finalList)
        {
            //livingEntity.HandleDamage(livingEntity.CalculateDamage(damage, livingEntity, attacker), attacker, true);
            HandleDamage(CalculateDamage(damage, livingEntity, attacker, damageType), attacker, livingEntity, true);
        }
    }

    public void HandleDamage(int damageAmount, LivingEntity attacker, LivingEntity victim, bool playVFXInstantly = false, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None, AbilityDataSO.DamageType damageType = AbilityDataSO.DamageType.None)
    {
        int blockAfter = victim.currentBlock;
        int healthAfter = victim.currentHealth;

        if (victim.currentBlock == 0)
        {
            healthAfter = victim.currentHealth - damageAmount;
            blockAfter = 0;
        }
        else if (victim.currentBlock > 0)
        {
            blockAfter = victim.currentBlock;
            Debug.Log("block after = " + blockAfter);
            blockAfter = blockAfter - damageAmount;
            Debug.Log("block after = " + blockAfter);
            if (blockAfter < 0)
            {
                healthAfter = victim.currentHealth;
                healthAfter += blockAfter;
                blockAfter = 0;
                Debug.Log("block after = " + blockAfter);
            }
        }

        if (victim.hasBarrier && healthAfter < victim.currentHealth)
        {
            damageAmount = 0;
            healthAfter = victim.currentHealth;
            victim.ModifyCurrentBarrierStacks(-1);
        }

        // Poisonous trait
        if (attacker.myPassiveManager.Poisonous && healthAfter < victim.currentHealth)
        {
            victim.ModifyPoison(attacker.myPassiveManager.poisonousStacks);
        }

        // Remove sleeping
        if (victim.isSleeping && damageAmount > 0)
        {
            Debug.Log("Damage taken, removing sleep");
            victim.ModifySleeping(-victim.currentSleepingStacks);
        }

        // Enrage
        if (victim.myPassiveManager.Enrage && healthAfter < victim.currentHealth)
        {
            Debug.Log("Enrage triggered, gaining strength");
            victim.ModifyCurrentStrength(victim.myPassiveManager.enrageStacks);
        }

        if (victim.myPassiveManager.Adaptive && healthAfter < victim.currentHealth)
        {
            Debug.Log("Adaptive triggered, gaining block");
            victim.ModifyCurrentBlock(victim.myPassiveManager.adaptiveStacks);
        }

        // remove camoflage if damaged
        if (victim.isCamoflaged && healthAfter < victim.currentHealth)
        {
            victim.RemoveCamoflage();
        }

        victim.currentHealth = healthAfter;
        victim.currentBlock = blockAfter;
        victim.myHealthBar.value = victim.CalculateHealthBarPosition();
        victim.UpdateCurrentHealthText();

        
        victim.SetCurrentBlock(victim.currentBlock);

        if (damageAmount > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateDamageEffect(victim.transform.position, damageAmount, playVFXInstantly));
        }

        if (victim.myPassiveManager.Thorns)
        {
            // TO DO: this needs be updated when we implement damage and attack types
            // if two characters with thorns attack each other, they will continously thorns damage each other until 1 dies
            // thorns damage can only be triggered by a melee attack
            if (attackType == AbilityDataSO.AttackType.Melee)
            {
                HandleDamage(CalculateDamage(victim.myPassiveManager.thornsStacks, victim, attacker, AbilityDataSO.DamageType.None), attacker, victim);
            }


        }

        if (victim.myPassiveManager.LightningShield)
        {
            // TO DO: this needs be updated when we implement damage and attack types
            // if two characters with thorns attack each other, they will continously thorns damage each other until 1 dies
            // thorns damage can only be triggered by a melee attack
            HandleDamage(CalculateDamage(victim.myPassiveManager.lightningShieldStacks, victim, attacker, AbilityDataSO.DamageType.Magic), attacker, victim);
        }

        if (victim.timesAttackedThisTurn == 0 && victim.myPassiveManager.QuickReflexes && victim.IsAbleToMove())
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Quick Reflexes", true));
            StartCoroutine(victim.StartQuickReflexesMove());
        }
        victim.timesAttackedThisTurn++;

        if (victim.currentHealth <= 0)
        {
            StartCoroutine(victim.HandleDeath());
        }
    }

    public int CalculateDamage(int abilityBaseDamage, LivingEntity victim, LivingEntity attacker, AbilityDataSO.DamageType damageType)
    {
        int newDamageValue = 0;

        // Establish base ability damage value
        newDamageValue += abilityBaseDamage;
        Debug.Log("Base damage value: " + newDamageValue);

        // add bonus strength
        newDamageValue += attacker.currentStrength;
        Debug.Log("Damage value after strength added: " + newDamageValue);

        // multiply/divide the damage value based on factors like vulnerable, knock down, magic vulnerability, etc
        newDamageValue = (int)(newDamageValue * CalculateAndGetDamagePercentageModifier(attacker, victim, damageType));
        Debug.Log("Damage value after percentage modifers like knockdown added: " + newDamageValue);

        return newDamageValue;
    }

    public float CalculateAndGetDamagePercentageModifier(LivingEntity attacker, LivingEntity victim, AbilityDataSO.DamageType damageType)
    {
        // Get damage type first
        AbilityDataSO.DamageType DamageType = damageType;

        // TO DO in future: this is where we modify the damage type based on character traits 
        // (e.g. if a character has a buff that makes all it damage types magical

        float damageModifier = 1f;
        if (victim.isKnockedDown)
        {
            damageModifier += 0.5f;
        }
        if (victim.myPassiveManager.Exposed)
        {
            damageModifier += 0.5f;
        }
        if (attacker.myPassiveManager.Exhausted)
        {
            damageModifier -= 0.5f;
        }
        /*
        if (PositionLogic.Instance.CanAttackerHitTargetsBackArc(attacker, victim))
        {
            damageModifier += 1f;
        }
        */
        if (victim.myPassiveManager.Flanked)
        {
            damageModifier += 0.5f;
        }
        if (victim.myPassiveManager.MagicImmunity && DamageType == AbilityDataSO.DamageType.Magic)
        {
            damageModifier = 0;
        }
        if (victim.myPassiveManager.PhysicalImmunity && DamageType == AbilityDataSO.DamageType.Physical)
        {
            damageModifier = 0;
        }

        // prevent modifier from going negative
        if (damageModifier < 0)
        {
            damageModifier = 0;
        }

        return damageModifier;

    }

    public bool IsTargetFriendly(LivingEntity caster, LivingEntity target)
    {
        Defender defender = caster.GetComponent<Defender>();
        Enemy enemy = caster.GetComponent<Enemy>();

        if (defender)
        {
            if (target.GetComponent<Defender>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (enemy)
        {
            if (target.GetComponent<Enemy>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            Debug.Log("CombatLogic.IsTargetFriendly() could not detect an enemy or defender script attached to the caster game object...");
            return false;
        }

    }

    // Method checks for a rune AND ALSO removes a rune when attempting to apply a debuff to target
    public bool IsProtectedByRune(LivingEntity target)
    {
        if (target.myPassiveManager.Rune)
        {
            target.myPassiveManager.ModifyRune(-1);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Method evaluates the difference between the attackers 'Aim' and the target's 'Defense', then returns the attackers hit probability as a percentage
    public int CalculateHitChance(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("CalculateHitChance() calculating hit probability between attacker " + attacker.name + " and " + target.name +"...");
        // Set up base hit chance properties
        int baseHitChance = 50;
        int attackerTotalAimValue = attacker.currentAim + baseHitChance;
        attackerTotalAimValue -= target.currentDefense;

        // Check for passive traits that modify defense or aim
        if (attacker.myPassiveManager.Entrenched)
        {
            Debug.Log("Attacker" + attacker.name + " is 'Entrenched', hit chance +10");
            attackerTotalAimValue += 10;

        }
        if (target.myPassiveManager.Entrenched)
        {
            Debug.Log("Attacker" + target.name + " is 'Entrenched', hit chance -10");
            attackerTotalAimValue -= 10;
        }

        // Add bonus defense if target is in half/full cover
        if(PositionLogic.Instance.IsTargetInHalfCover(attacker, target))
        {
            Debug.Log("Target is in half cover, -20 to attack hit chance probability...");
            attackerTotalAimValue -= 20;
        }
        else if (PositionLogic.Instance.IsTargetInFullCover(attacker, target))
        {
            Debug.Log("Target is in full cover, -40 to attack hit chance probability...");
            attackerTotalAimValue -= 40;
        }
        else
        {
            Debug.Log("Target is not in cover...");
        }

        // attacks can never be 0% hit chance. They wil always be atleast 10% chance to hit
        if (attackerTotalAimValue < 10)
        {
            attackerTotalAimValue = 10;
        }
        else if (attackerTotalAimValue > 100)
        {
            attackerTotalAimValue = 100;
        }

        Debug.Log(" CalculateHitChanceFromAimVsDefense() calculated that " + attacker.name + " has a " + attackerTotalAimValue + "% chance of hitting " + target.name + " with an attack");
        return attackerTotalAimValue;
    }

    public bool CalculateIfAttackHitOrMiss(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("CombatLogic.CalculateIfAttackHitOrMiss() called...");
        int hitChance = CalculateHitChance(attacker, target);

        int randomNumber = Random.Range(1, 101);
        Debug.Log("CombatLogic.CalculateIfAttackHitOrMiss() rolled a " + randomNumber.ToString());

        if (randomNumber >= 1 && randomNumber <= hitChance)
        {
            // Attack hit
            Debug.Log("CombatLogic.CalculateIfAttackHitOrMiss() attack hit, " + randomNumber.ToString() + " is less then " + hitChance.ToString());
            return true;
        }
        else
        {
            // Attack miss
            Debug.Log("CombatLogic.CalculateIfAttackHitOrMiss() attack missed, " + randomNumber.ToString() + " is more then " + hitChance.ToString());
            return false;
        }
    }
   

}

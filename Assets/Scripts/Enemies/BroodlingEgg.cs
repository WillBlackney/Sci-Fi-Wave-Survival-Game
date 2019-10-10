using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodlingEgg : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Hatch Broodling");

    }

    public override IEnumerator StartMyActivationCoroutine()
    {        
        Action action = AbilityLogic.Instance.PerformHatchBroodling(this);
        yield return new WaitForSeconds(1f);
        EndMyActivation();
        yield return null;
    }

    
}

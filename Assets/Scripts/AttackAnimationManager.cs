using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : Singleton<AttackAnimationManager>
{
    [Header("Projectile Prefabs")]
    public GameObject bulletPrefab;
    public GameObject bulletTracerPrefab;

    public Action PlayAttackAnimation(LivingEntity attacker, LivingEntity target, Ability ability = null)
    {
        Action action = new Action();
        StartCoroutine(PlayAttackAnimationCoroutine(attacker, target, action, ability));
        return action;
    }

    public IEnumerator PlayAttackAnimationCoroutine(LivingEntity attacker, LivingEntity target, Action action, Ability ability = null)
    {
        // Shoot
        if(ability != null && ability.abilityName == "Shoot")
        {
            // Multi fire weapons
            if(attacker.myRangedWeapon.weaponName == "Assault Rifle")
            {
                for(int shotsTaken = 0; shotsTaken < 3; shotsTaken++)
                {
                    attacker.myAnimator.SetTrigger("ShootWeapon");
                    //GameObject newTracer = Instantiate(bulletTracerPrefab);
                    //newTracer.GetComponent<BulletTracer>().Setup(attacker, target); 
                    GameObject newBullet = Instantiate(bulletPrefab);
                    newBullet.GetComponent<Projectile>().Setup(target, attacker);
                    GameObject newTracer = Instantiate(bulletTracerPrefab, newBullet.transform);
                    newTracer.GetComponent<BulletTracer>().Setup(attacker, target, newBullet);
                    yield return new WaitUntil(() => attacker.ShootAnimationFinished() == true);
                    attacker.shootAnimationFinished = false;
                }                   
            }


            // Single shot weapons
        }

        action.actionResolved = true;
    }
}

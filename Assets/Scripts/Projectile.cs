using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LivingEntity myTarget;
    public LivingEntity myCreator;
    public float speed;
    public bool destinationReached;

    public void Setup(LivingEntity target, LivingEntity creator)
    {
        transform.position = creator.TileCurrentlyOn.WorldPosition;
        myTarget = target;
        myCreator = creator;        
    }

    private void Update()
    {
        if(myTarget != null)
        {
            MoveTowardsTarget();
        }
    }

    public void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, myTarget.transform.position, speed * Time.deltaTime);
        if(transform.position == myTarget.transform.position)
        {
            destinationReached = true;
            Destroy(gameObject);
        }
    }
}

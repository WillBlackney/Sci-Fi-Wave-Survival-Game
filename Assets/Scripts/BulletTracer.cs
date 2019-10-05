using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject projectileParent;
    public void Setup(LivingEntity attacker, LivingEntity target)
    {
        lineRenderer.SetPosition(0, target.transform.position);
        lineRenderer.SetPosition(1, attacker.transform.position);
        Destroy(gameObject, 0.1f);
    }

    public void Setup(LivingEntity attacker, LivingEntity target, GameObject parent)
    {
        
        lineRenderer.SetPosition(0, parent.transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
        projectileParent = parent;
        //Destroy(gameObject, 0.1f);
    }

    private void Update()
    {
        if(projectileParent != null)
        {
            lineRenderer.SetPosition(0, projectileParent.transform.position);
        }
    }


}

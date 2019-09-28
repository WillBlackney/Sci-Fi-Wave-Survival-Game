using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject myParent;
    public void DestroyMyself()
    {
        Destroy(myParent);
    }
}

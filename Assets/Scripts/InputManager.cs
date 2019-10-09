using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            UnselecteDefender();
        }
    }

    public void UnselecteDefender()
    {
        if(DefenderManager.Instance.selectedDefender != null)
        {
            DefenderManager.Instance.ClearSelectedDefender();
        }
    }
}

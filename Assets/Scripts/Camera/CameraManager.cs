using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject currentLookAtTarget;
    public Vector3 currentLookAtPosition;
    public bool lookAtPosition;

    public void SetCameraLookAtTarget(GameObject target)
    {
        currentLookAtTarget = target;
    }
    public void ClearCameraLookAtTarget()
    {
        currentLookAtTarget = null;
    }
    public void SetCameraLookAtPosition(Vector3 location)
    {
        lookAtPosition = true;
        currentLookAtPosition = location;
    }
    public void ClearCameraLookAtPosition()
    {
        lookAtPosition = false;
    }

    public bool IsCameraWithinRangeOfTarget(GameObject target, float range = 1)
    {
        // x range
        float xDistance = mainCamera.transform.position.x - target.transform.position.x;
        float yDistance = mainCamera.transform.position.y - target.transform.position.y;
        if (xDistance > range || xDistance < -range || yDistance > range || yDistance < -range)
        {
            return false;
        }
        else
        {
            Debug.Log("Camera is now within " + range.ToString() + " of " + target.name);
            return true;
        }
        //if (mainCamera.transform.position.x >= target.transform.position.x)
    }
}

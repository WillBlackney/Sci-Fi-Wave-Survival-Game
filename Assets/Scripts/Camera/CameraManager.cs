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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject currentLookAtTarget;


    public void LookAtTarget(GameObject target)
    {
        SetCameraLookAtTarget(target);
        mainCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, mainCamera.transform.position.z);
    }
    public void SetCameraLookAtTarget(GameObject target)
    {
        //cinemachineCamera.LookAt = target.transform;
        //cinemachineCamera.Follow = target.transform;
        currentLookAtTarget = target;
    }

    public void ClearCameraLookAtTarget()
    {
        //cinemachineCamera.LookAt = null;
        //cinemachineCamera.Follow = null;
        currentLookAtTarget = null;
    }
}

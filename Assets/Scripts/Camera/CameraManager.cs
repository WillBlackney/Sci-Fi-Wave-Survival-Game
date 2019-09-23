﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject currentLookAtTarget;

    public void SetCameraLookAtTarget(GameObject target)
    {        
        currentLookAtTarget = target;
    }
    public void ClearCameraLookAtTarget()
    {
        currentLookAtTarget = null;
    }
}

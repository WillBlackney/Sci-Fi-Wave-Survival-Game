using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 1f;
    public float cameraZoomSpeed;
    public Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cam.fieldOfView -= cameraZoomSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cam.fieldOfView += cameraZoomSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * cameraMoveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * Time.deltaTime * cameraMoveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * cameraMoveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * cameraMoveSpeed;

        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FPS : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _sensitivity = 1.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(rotateVertical * _sensitivity, -rotateHorizontal * _sensitivity, 0);

        _camera.transform.localEulerAngles -= rotation;
    }
}

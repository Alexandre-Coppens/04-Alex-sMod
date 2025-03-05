using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Player")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _speedVelocity = 0.1f;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpStrength = 5.0f;
    [SerializeField] private Vector3 _nextMovement;

    [Header("Physics")]
    [SerializeField] private ObjectGravity _playerGravity;

    void Start()
    {
        _playerGravity = GetComponent<ObjectGravity>();
    }

    void Update()
    {
        InputKeys();
        Roation();
    }
    
    void InputKeys()
    {
        float x = 0;
        float z = 0;
        float _speedVelDelta = _speedVelocity * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) { z += _speedVelDelta; }
        if (Input.GetKey(KeyCode.S)) { z -= _speedVelDelta; }
        if (Input.GetKey(KeyCode.A)) { x -= _speedVelDelta; }
        if (Input.GetKey(KeyCode.D)) { x += _speedVelDelta; }

        if (_playerGravity.grounded == true) 
        {
            if (Input.GetKey(KeyCode.Space)) { _playerGravity.AddImpulse(new Vector3(0, _jumpStrength, 0)); }
            if (x == 0) { _playerGravity.velocity.x = Mathf.MoveTowards(_playerGravity.velocity.x, 0, _speedVelDelta); }
            if (z == 0) { _playerGravity.velocity.z = Mathf.MoveTowards(_playerGravity.velocity.z, 0, _speedVelDelta); }
        }

        _playerGravity.velocity += (new Vector3(x, 0, z));
        _playerGravity.velocity.x = Mathf.Clamp(_playerGravity.velocity.x, -_speed, _speed);
        _playerGravity.velocity.z = Mathf.Clamp(_playerGravity.velocity.z, -_speed, _speed);
    }

    void Roation()
    {
        //transform.localEulerAngles = new Vector3(0, _camera.transform.localEulerAngles.y);
        //_playerModel.transform.localEulerAngles = new Vector3(0, _camera.transform.localEulerAngles.y);
        //transform.forward = _playerModel.transform.forward;
        //_playerModel.transform.localRotation = new Quaternion();
        //_camera.transform.localEulerAngles = new Vector3(_camera.transform.localEulerAngles.x, 0,_camera.transform.localEulerAngles.z);
    }
}
    
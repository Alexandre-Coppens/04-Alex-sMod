using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    void Start()
    {
        Cameraman camScript = cam.GetComponent<Cameraman>();
        GunScript gunScript = gun.GetComponent<GunScript>();
        gunTimer = gunScript.timeShoot;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    

    void Update()
    {
        _velocity = new Vector3((Input.GetKey(iForward) ? 1 : 0) + (Input.GetKey(iBackward) ? -1 : 0), 0, (Input.GetKey(iLeft) ? -1 : 0) + (Input.GetKey(iRight) ? 1 : 0));
        _direction = ((cam.transform.forward * _velocity.x) + (cam.transform.right * _velocity.z));
        _direction.y = 0f;

        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        Vector3 _rotation = new Vector3(rotateVertical * xSensitivity, -rotateHorizontal * ySensitivity, 0);
        cam.transform.eulerAngles -= _rotation;

        isRunning = Input.GetKey(iSprint) && canRun;
        hasJumped = Input.GetKeyDown(KeyCode.Space);

        if (camScript != null) { camScript.CamUpdate(rb.velocity, isRunning, hasJumped); }
        else { camScript = cam.GetComponent<Cameraman>(); }

        if (gunScript != null) { GunManagement(gun, gunScript); }
        else { gunScript = gun.GetComponent<GunScript>(); }
        

        value1 = rotateHorizontal;
        value2 = rotateVertical;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGrounded) { isGrounded = true; }
    }

    private void FixedUpdate()
    {
        rb.velocity = (_direction * speed * (isRunning && _velocity == new Vector3(1, 0, 0) ? sprintMultiplier : 1)) + new Vector3(0,rb.velocity.y);
        if (hasJumped && isGrounded) { rb.velocity += new Vector3(0,jumpStrength); isGrounded = false; }
    }

    [HideInInspector] public bool isShooting = false;
    [HideInInspector] public float timeBtwShots = 0;
    private void GunManagement(GameObject gun, GunScript gunScript)
    {
        if (Time.realtimeSinceStartup - timeBtwShots >= gunTimer)
        {
            canRun = true;
            isShooting = Input.GetKey(iShoot);
            gunScript.GunUpdate(isRunning, isShooting, Time.realtimeSinceStartup);
            if (isShooting)
            {
                timeBtwShots = Time.realtimeSinceStartup;
                canRun = false;
            }            
        }
    }
}   
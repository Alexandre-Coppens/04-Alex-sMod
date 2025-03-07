using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    Player_Inputs player_Inputs;
    
    [Header("Movements")]
    [SerializeField] private Vector2 movements = Vector2.zero;
    [SerializeField] private Vector2 cameraAxis = Vector2.zero;
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float playerSpeedAcceleration = 5;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private float floorDistance;
    private float gravity = 9.81f;
    private float gravityMultiplicator = 1f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 camRotation = Vector3.zero;

    [Header("Jump")]
    [SerializeField] private bool jump = false;
    [SerializeField] private float jumpStrength = 5f;
    [Tooltip("X = Jump Multiplicator, Y = Fall Multiplicator")]
    [SerializeField] private Vector2 jumpGravityMultiplicator = Vector2.zero;

    [Header("Player && Movements")]
    [SerializeField] private GameObject camera;
    [SerializeField, Range(0.01f, 10f)] private float xSensitivity = 1f;
    [SerializeField, Range(0.01f, 10f)] private float ySensitivity = 1f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Weapon")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GunScript gunScript;
    [SerializeField] private bool isShooting;

    [Header("Debug")]
    public bool hasJumped = false;
    public bool isOnGround = true;
    public bool isSprinting = false;
    [SerializeField] float value1;
    [SerializeField] float value2;
    [SerializeField] float value3;

    private Rigidbody rb;
    private Animator cameraAnimator;

    private bool canSprint = true;
    [HideInInspector] float gunTimer;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player_Inputs = Player_Inputs.instance;
        rb = GetComponent<Rigidbody>();
        camera = Camera.main.gameObject;
        cameraAnimator = camera.GetComponent<Animator>();
        gunScript = gun.GetComponent<GunScript>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInputs();
        Move();
        Animations();
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    private void GetInputs()
    {
        movements = player_Inputs.movement;
        jump = player_Inputs.jumpPressed;
        isSprinting = player_Inputs.runPressed;
        cameraAxis = new Vector2(Input.GetAxis("Mouse X") + player_Inputs.camera.x, Input.GetAxis("Mouse Y") + player_Inputs.camera.y);
        isOnGround = Physics.Raycast(transform.position - Vector3.down * 0.05f, Vector3.down, floorDistance, floorLayer);
        isShooting = player_Inputs.shootPressed;
    }

    private void Move()
    {
        velocity = transform.forward * movements.y + transform.right * movements.x;
        transform.position += velocity * Time.deltaTime * playerSpeed * (isSprinting ? sprintMultiplier : 1);

        camRotation = new Vector3(cameraAxis.y * xSensitivity, -cameraAxis.x * ySensitivity, 0);
        camera.transform.rotation = Quaternion.Euler(MathInvClamp(camera.transform.eulerAngles.x - camRotation.x, 80, 270), camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
        transform.eulerAngles -= new Vector3(0, camRotation.y, 0);

        if (jump)
        {
            if(!hasJumped && isOnGround)
            {
                Debug.Log("Jumped");
                rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
                gravityMultiplicator = jumpGravityMultiplicator.x;
            }
            hasJumped = true;
        }
        else
        {
            hasJumped = false;
        }
        if (rb.velocity.y < 1 && gravityMultiplicator != jumpGravityMultiplicator.y) gravityMultiplicator = jumpGravityMultiplicator.y;
    }

    private void Animations()
    {
        gunScript.GunUpdate(isSprinting, isShooting);
        cameraAnimator.SetBool("isRunning", isSprinting);
        cameraAnimator.SetFloat("Velocity", Mathf.Abs(movements.x) + Mathf.Abs(movements.y));
    }

    private void Gravity()
    {
        if (isOnGround) gravityMultiplicator = 1f;
        rb.AddForce((Vector3.down * (gravity * gravityMultiplicator)), ForceMode.Acceleration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position - Vector3.down * 0.05f, Vector3.down * floorDistance);
    }

    //Returns value if inferior or equal to min or superior or equal to max. Else returns the closest.
    private float MathInvClamp(float value, float min, float max)
    {
        if (value <= min) return value;
        if (value >= max) return value;
        float x = max - min;
        if ( value - x >= 0) return max;
        return min;
    }
}

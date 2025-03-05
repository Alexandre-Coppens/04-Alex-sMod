using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GPlayer : MonoBehaviour
{
    Player_Inputs player_Inputs;
    [Header("Movements")]
    [SerializeField] private Vector2 movements = Vector2.zero;
    [SerializeField] private Vector2 mouseAxis = Vector2.zero;
    [SerializeField] private bool jump = false;
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float playerSpeedAcceleration = 5;
    private Vector3 velocity = Vector3.zero;
    private Vector3 camRotation = Vector3.zero;

    [Header("Player && Movements")]
    [SerializeField] private GameObject camera;
    [SerializeField, Range(0.01f, 10f)] private float xSensitivity = 1f;
    [SerializeField, Range(0.01f, 10f)] private float ySensitivity = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpStrength = 5f;
    [SerializeField] private GameObject gun;
    [Tooltip ("X = Normal Mass (get on start), Y = Jump Multiplicator, Z = Fall Multiplicator")]
    [SerializeField] private Vector3 jumpGravityMultiplicator = Vector3.zero;

    [Header("Debug")]
    public bool hasJumped = false;
    public bool isOnGround = true;
    public bool isSprinting = false;
    [SerializeField] CapsuleCollider collider;

    private Rigidbody rb;

    private bool canSprint = true;
    [HideInInspector] float gunTimer;

    void Start()
    {
        player_Inputs = Player_Inputs.instance;
        rb = GetComponent<Rigidbody>();
        camera = Camera.main.gameObject;

        jumpGravityMultiplicator.x = rb.mass;
        collider = GetComponent<CapsuleCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInputs();
        Move();
    }

    private void GetInputs()
    {
        movements = player_Inputs.movement;
        jump = player_Inputs.jumpPressed;
        isSprinting = player_Inputs.runPressed;
        mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        CheckGround();
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + collider.center, Vector3.down, out hit, 10f, groundLayer))
        {
            if (hit.distance > collider.height * 0.5f + 0.01f) return;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            transform.position = hit.point + collider.height * 0.5f * Vector3.up;
            isOnGround = true;
        }
    }

    private void Move()
    {
        velocity = transform.forward * movements.y + transform.right * movements.x;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity * playerSpeed * (isSprinting ? sprintMultiplier : 1);

        camRotation = new Vector3(mouseAxis.y * xSensitivity, -mouseAxis.x * ySensitivity, 0);
        camera.transform.rotation = Quaternion.Euler(MathInvClamp(camera.transform.eulerAngles.x - camRotation.x, 80, 270), camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
        transform.eulerAngles -= new Vector3(0, camRotation.y, 0);

        if (jump && isOnGround)
        {
            rb.AddForce(0, jumpStrength, 0);
            transform.position += new Vector3(0, 0.1f, 0);
            rb.mass = jumpGravityMultiplicator.x * jumpGravityMultiplicator.y;
            isOnGround = false;
        }
        if (rb.velocity.y < 1 && rb.mass != jumpGravityMultiplicator.x * jumpGravityMultiplicator.z) rb.mass = jumpGravityMultiplicator.x * jumpGravityMultiplicator.z;
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

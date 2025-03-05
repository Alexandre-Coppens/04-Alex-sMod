using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Player3D : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private Vector2 movements = Vector2.zero;
    [SerializeField] private Vector2 speed = Vector2.zero;
    [SerializeField] private Vector2 mouseAxis = Vector2.zero;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float runTime = 1.5f;
    [SerializeField] private bool isSprinting = false;

    [Header("Jump")]
    [SerializeField] private bool jumpPressed;
    public bool allowedToJump;
    public bool isJumping;
    [SerializeField] private float jumpBufferMaxTime, currentJumpBufferTime;
    public float jumpUpMaxTime ,currentJumpUpTime;
    [SerializeField] private int playerMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxCoyoteTime, currentCoyoteTime;
    [Tooltip("X = Normal, Y = Jumping, Z = Falling")]
    [SerializeField] private Vector3 gravityJump = Vector3.one;

    [Header("VFX")]
    [SerializeField] private VisualEffect walkVFX;
    [SerializeField] private VisualEffect runVFX;
    [SerializeField] private VisualEffect dashVFX;

    [Header("Elements")]
    [SerializeField] private Player_Inputs player_Inputs;
    [SerializeField] private ObjectGravity objectGravity;
    [SerializeField] private GameObject camera;
    public static Player3D instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        player_Inputs = Player_Inputs.instance;
        objectGravity = GetComponent<ObjectGravity>();
        camera = Camera.main.gameObject;

        InputSystem.pollingFrequency = 120;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInputs();
        Movement();
        Jump();
        Vfx();
    }

    private void GetInputs()
    {
        if (Vector2.Distance(Vector2.zero, player_Inputs.movement) > Vector2.Distance(Vector2.zero, movements)) movements = Vector2.MoveTowards(movements, player_Inputs.movement, Time.deltaTime * 2);
        else movements = Vector2.MoveTowards(movements, player_Inputs.movement, Time.deltaTime * 5);
        jumpPressed = player_Inputs.jumpPressed;
        isSprinting = player_Inputs.runPressed;
        mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void Movement()
    {
        transform.eulerAngles += new Vector3(0, mouseAxis.x, 0);
        camera.transform.eulerAngles -= new Vector3(mouseAxis.y, 0);

        Vector3 direction = transform.forward * movements.y + transform.right * movements.x;
        direction.Normalize();

        float _speed = isSprinting ? speed.x : speed.y;
        objectGravity.addedMovement = direction * _speed;

        if (CheckGround()) { objectGravity.objectMass = gravityJump.x; }
        else
        {
            if (objectGravity.velocity.y < -0.1f) objectGravity.objectMass = gravityJump.z;
        }
     }

    private bool CheckGround()
    {
        return objectGravity.grounded;
    }

    private void Jump()
    {
        if (jumpPressed)
        {
            if(!allowedToJump) { currentJumpBufferTime = Time.time; allowedToJump = true; }
            if(currentJumpBufferTime >= Time.time - jumpBufferMaxTime && CheckGround())
            {
                objectGravity.objectMass = gravityJump.y;
                objectGravity.AddImpulse(new Vector3(0, jumpForce * objectGravity.gravity, 0));
                currentJumpBufferTime = Time.time;
                isJumping = true;
            }
            if (isJumping && currentJumpBufferTime >= Time.time - jumpBufferMaxTime)
            {
                objectGravity.AddImpulse(new Vector3(0, jumpForce * Time.deltaTime, 0));
            }
        }
        else
        {
            currentJumpBufferTime = Mathf.Infinity;
            allowedToJump = false;
            isJumping = false;
        }
    }

    private void Vfx()
    {
        if (CheckGround())
        {
            if (isSprinting && Vector2.Distance(Vector2.zero, player_Inputs.movement) > 0.1f) { runVFX.Play(); }
            else if (Vector2.Distance(Vector2.zero, player_Inputs.movement) > 0.1f) { walkVFX.Play(); }
            else { walkVFX.Stop(); runVFX.Stop(); }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Player_Inputs player_Inputs;
    [Header("Movements")]
    [SerializeField] private Vector2 movements = Vector2.zero;
    [SerializeField] private bool jump = false;
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float accelerationSpeed = 5;
    private Vector3 addMovements = Vector3.zero;

    [Header("Player && Movements")]
    [SerializeField] GameObject cam;
    [SerializeField, Range(0.01f, 10f)] float xSensitivity = 1f;
    [SerializeField, Range(0.01f, 10f)] float ySensitivity = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float jumpStrength = 5f;
    [SerializeField] GameObject gun;

    [Header("Debug")]
    public bool hasJumped = false;
    public bool isGrounded = true;
    public bool isRunning = false;
    [SerializeField] float value1;
    [SerializeField] float value2;
    [SerializeField] float value3;

    private Rigidbody rb;

    [HideInInspector] public bool canRun = true;
    [HideInInspector] float gunTimer;
    Vector3 _velocity = Vector3.zero;
    Vector3 _direction = Vector3.zero;

    void Start()
    {
        player_Inputs = Player_Inputs.instance;
        rb = GetComponent<Rigidbody>();
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
    }

    private void Move()
    {
        addMovements = transform.forward * movements.y + transform.right * movements.x * Time.deltaTime * playerSpeed;
        transform.position += addMovements;
    }
}

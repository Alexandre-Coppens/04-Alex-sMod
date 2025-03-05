using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Inputs : MonoBehaviour
{
    PlayerInput InputAction;
    public static Player_Inputs instance;

    [HideInInspector] public bool jumpPressed = false;
    [HideInInspector] public bool shootPressed = false;
    [HideInInspector] public bool aimPressed = false;
    [HideInInspector] public bool reloadPressed = false;
    [HideInInspector] public bool runPressed = false;
    [HideInInspector] public Vector2 movement = Vector2.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ForwardBackward(InputAction.CallbackContext context)
    {
        movement.y = context.ReadValue<float>();
    }

    public void LeftRight(InputAction.CallbackContext context)
    {
        movement.x = context.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        shootPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void Aim(InputAction.CallbackContext context)
    {
        aimPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void Run(InputAction.CallbackContext context)
    {
        runPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void Reload(InputAction.CallbackContext context)
    {
        reloadPressed = context.ReadValue<float>() > 0 ? true : false;
    }
}
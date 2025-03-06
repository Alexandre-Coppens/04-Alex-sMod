using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Inputs : MonoBehaviour
{
    PlayerInput InputAction;
    public static Player_Inputs instance;

    public bool jumpPressed = false;
    public bool shootPressed = false;
    public bool aimPressed = false;
    public bool reloadPressed = false;
    public bool runPressed = false;
    public Vector2 movement = Vector2.zero;
    public Vector2 camera = Vector2.zero;

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

    public void Jump(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void ForwardBackward(InputAction.CallbackContext context)
    {
        movement.y = context.ReadValue<float>();
    }

    public void LeftRight(InputAction.CallbackContext context)
    {
        movement.x = context.ReadValue<float>();
    }

    public void CameraRotation(InputAction.CallbackContext context)
    {
        camera = context.ReadValue<Vector2>();
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
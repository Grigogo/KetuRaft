using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Sprint { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool Crouch { get; private set; }

    private InputSystem_Actions actions;

    private void Awake()
    {
        actions = new InputSystem_Actions();
    }

    private void OnEnable()  => actions.Player.Enable();
    private void OnDisable() => actions.Player.Disable();

    private void Update()
    {
        Move        = actions.Player.Move.ReadValue<Vector2>();
        Look        = actions.Player.Look.ReadValue<Vector2>();
        Sprint      = actions.Player.Sprint.IsPressed();
        JumpPressed = actions.Player.Jump.WasPressedThisFrame();
        Crouch      = actions.Player.Crouch.IsPressed();
    }
}
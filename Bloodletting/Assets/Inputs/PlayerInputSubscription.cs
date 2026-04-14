using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSubscription : MonoBehaviour
{
    public static PlayerInputSubscription Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        enabled = true;
    }
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;

    public bool AttackInput { get; private set; } = false;
    public bool AltAttackInput { get; private set; } = false;

    Player_TopDown _Input = null;

    private void OnEnable()
    {
        _Input = new Player_TopDown();
        _Input.Player.Enable();

        _Input.Player.Move.performed += SetMovement;
        _Input.Player.Move.canceled += SetMovement;

        _Input.Player.Attack.performed += SetAttack;
        _Input.Player.Attack.canceled += SetAttack;

        _Input.Player.AltAttack.performed += SetAltAttack;
        _Input.Player.AltAttack.canceled += SetAltAttack;

        _Input.Player.Look.performed += SetLook;
        _Input.Player.Look.canceled += SetLook;
    }
    private void OnDisable()
    {
        _Input.Player.Move.performed -= SetMovement;
        _Input.Player.Move.canceled -= SetMovement;

        _Input.Player.Attack.performed -= SetAttack;
        _Input.Player.AltAttack.canceled -= SetAltAttack;

        _Input.Player.AltAttack.performed -= SetAltAttack;
        _Input.Player.AltAttack.canceled -= SetAltAttack;

        _Input.Player.Look.performed -= SetLook;
        _Input.Player.Look.canceled -= SetLook;

        _Input.Player.Disable();
    }

    void SetMovement(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    void SetLook(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Mouse)
        {
            LookInput = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        else
            LookInput = ctx.ReadValue<Vector2>();
    }
    void SetAltAttack(InputAction.CallbackContext ctx)
    {
        AltAttackInput = ctx.ReadValueAsButton();
    }
    void SetAttack(InputAction.CallbackContext ctx)
    {
        AttackInput = ctx.ReadValueAsButton();
    }
}

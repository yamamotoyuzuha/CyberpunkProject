using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーが操作するキャラクター
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの使用するInputSystem
    /// </summary>
    public PlayerInputSystem PlayerInputSystem { get; private set; }

    [Header("CharacterSO")] [SerializeField] private CharacterSO _characterSo;

    private Rigidbody _rb;
    private Vector2 _moveInput;

    private void Awake()
    {
        PlayerInputSystem = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        PlayerInputSystem.Enable();
        PlayerInputSystem.Player.Move.performed += OnMove;
        PlayerInputSystem.Player.Move.canceled += OnMoveCancel;
    }

    private void OnDisable()
    {
        PlayerInputSystem.Disable();
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        // ダッシュボタンが押されている場合、走り速度を使用する
        float speed = PlayerInputSystem.Player.Dash.IsPressed()
            ? _characterSo.DashSpeed
            : _characterSo.WalkSpeed;
        
        var linearVelocity = new Vector3(_moveInput.x * speed, 0, _moveInput.y * speed);
        _rb.linearVelocity = linearVelocity;
        DesignatedDirectionRotation(linearVelocity);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    
    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    /// <summary>
    /// 指定方向へとプレイヤーを回転させる
    /// </summary>
    /// <param name="direction">回転させる方向</param>
    public void DesignatedDirectionRotation(Vector3 direction)
    {
        direction.y = 0;
        if(direction == Vector3.zero) return;
        
        // 指定方向へと回転
        _rb.rotation = Quaternion.LookRotation(direction);
    }
}

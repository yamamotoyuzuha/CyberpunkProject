using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーが操作するキャラクター
/// </summary>
public class Player : CharacterBase
{
    /// <summary>
    /// プレイヤーの使用するInputSystem
    /// </summary>
    public PlayerInputSystem PlayerInputSystem { get; private set; }

    [Header("CharacterSO"), SerializeField] private CharacterSO _characterSo;
    [Header("地面のレイヤー"), SerializeField] private LayerMask _groundLayer;
    [Header("地面を判定するRayの長さ"), SerializeField] private float _rayDistance;

    private Camera _mainCamera;
    private Rigidbody _rb;
    private Vector2 _moveInput;
    
    private void Awake()
    {
        PlayerInputSystem = new PlayerInputSystem();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        PlayerInputSystem.Enable();
        PlayerInputSystem.Player.Move.performed += OnMove;
        PlayerInputSystem.Player.Move.canceled += OnMoveCancel;
        PlayerInputSystem.Player.Jump.performed += OnJump;
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
        
        // カメラの前と右を取得
        var forward = _mainCamera.transform.forward;
        var right = _mainCamera.transform.right;
        // カメラを考慮した、移動方向を作成
        var move = (_moveInput.x * right + _moveInput.y * forward).normalized;
        
        var linearVelocity = new Vector3(move.x * speed, _rb.linearVelocity.y, move.z * speed);
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

    private void OnJump(InputAction.CallbackContext context)
    {
        // 二重ジャンプを防止する
        if(!context.performed || !GroundCheck()) return;
        _rb.AddForce(Vector3.up * _characterSo.JumpPower, ForceMode.Impulse);
    }
    
    /// <summary>
    /// 地面の判定
    /// </summary>
    /// <returns>true：地面についてる　false：地面についていない</returns>
    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, _rayDistance, _groundLayer);
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

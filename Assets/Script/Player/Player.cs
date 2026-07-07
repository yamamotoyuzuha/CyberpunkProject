using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーが操作するキャラクター
/// </summary>
public class Player : CharacterBase, ICharacterAnimator
{
    /// <summary>
    /// プレイヤーの使用するInputSystem
    /// </summary>
    public PlayerInputSystem PlayerInputSystem { get; private set; }

    [Header("CharacterSO"), SerializeField] private CharacterSO _characterSo;
    [Header("地面のレイヤー"), SerializeField] private LayerMask _groundLayer;
    [Header("地面を判定するRayの長さ"), SerializeField] private float _rayDistance;
    [Header("アニメーションの設定")]
    [SerializeField] private float _walkBlendValue = 0.5f;
    [SerializeField] private float _dashBlendValue = 1f;
    
    private Camera _mainCamera;
    private Rigidbody _rb;
    private Animator _animator;
    
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    private CharacterStatus _characterStatus;
    /// <summary>
    /// プレイヤーの入力
    /// </summary>
    private Vector2 _moveInput;

    /// <summary>
    /// 移動アニメーションのBlend値
    /// </summary>
    private float _moveBlendValue;
    
    private void Awake()
    {
        PlayerInputSystem = new PlayerInputSystem();
        _mainCamera = Camera.main;

        // キャラクターのステータスを作成
        _characterStatus = new CharacterStatus(_characterSo.Hp, _characterSo.Stamina);
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
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove();
    }

    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    private void PlayerMove()
    {
        // ダッシュボタンが押されている場合、走り速度を使用する
        (float moveSpeed, bool isWalking) movement = PlayerInputSystem.Player.Dash.IsPressed()
            ? (_characterSo.DashSpeed, false)
            : (_characterSo.WalkSpeed, true);
        
        // カメラの前と右を取得
        var forward = _mainCamera.transform.forward;
        var right = _mainCamera.transform.right;
        // カメラを考慮した、移動方向を作成
        var move = (_moveInput.x * right + _moveInput.y * forward).normalized;
        
        var linearVelocity = new Vector3(move.x * movement.moveSpeed, _rb.linearVelocity.y, move.z * movement.moveSpeed);
        _rb.linearVelocity = linearVelocity;
        DesignatedDirectionRotation(linearVelocity);

        // 移動状態に応じて、移動アニメーション用のパラメータを設定する
        if (move == Vector3.zero) _moveBlendValue = 0;
        else _moveBlendValue = movement.isWalking ? _walkBlendValue : _dashBlendValue;
        SetMoveAnimation(_moveBlendValue, movement.isWalking);
    }

    public void SetMoveAnimation(float moveParam, bool isFlag)
    {
        _animator.SetFloat("Move", moveParam);
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

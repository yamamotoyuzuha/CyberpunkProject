using UnityEngine;

/// <summary>
/// プレイヤーの動きを管理するクラス
/// ・移動処理
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerInputHandler"), SerializeField] private PlayerInputHandler _playerInputHandler;
    [Header("CharacterAnimationController"), SerializeField] private CharacterAnimationController _animController;
    [Header("歩き速度"), SerializeField] private float _walkSpeed = 3f;
    [Header("走り速度"), SerializeField] private float _dashSpeed = 10f;
    [Header("ジャンプ速度"), SerializeField] private float _jumpSpeed = 5f;
    [Header("地面のレイヤー"), SerializeField] private LayerMask _groundLayer;
    [Header("地面を判定するRayの長さ"), SerializeField] private float _rayDistance;
    [Header("アニメーションのパラメータ設定")]
    [SerializeField] private float _walkBlendValue = 0.5f;
    [SerializeField] private float _dashBlendValue = 1f;
    
    private Camera _mainCamera;
    private Rigidbody _rb;
    
    /// <summary>
    /// 移動アニメーションのBlend値
    /// </summary>
    private float _moveBlendValue;

    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _playerInputHandler.JumpAction += Jump;
    }

    private void OnDisable()
    {
        _playerInputHandler.JumpAction -= Jump;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // ダッシュボタンが押されている場合、走り速度を使用する
        (float moveSpeed, bool isWalking) movement = _playerInputHandler.IsDashInput
            ? (_dashSpeed, false) : (_walkSpeed, true);
        
        // カメラの前と右を取得
        var forward = _mainCamera.transform.forward;
        var right = _mainCamera.transform.right;
        // カメラを考慮した、移動方向を作成
        var move = (_playerInputHandler.MoveInput.x * right + _playerInputHandler.MoveInput.y * forward).normalized;

        var linearVelocity = new Vector3(move.x * movement.moveSpeed, _rb.linearVelocity.y, move.z * movement.moveSpeed);
        _rb.linearVelocity = linearVelocity;
        DesignatedDirectionRotation(linearVelocity);
        
        // 移動状態に応じて、移動アニメーション用のパラメータを設定する
        if (move == Vector3.zero) _moveBlendValue = 0;
        else _moveBlendValue = movement.isWalking ? _walkBlendValue : _dashBlendValue;
        _animController.SetMoveAnimation(_moveBlendValue, movement.isWalking);
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump()
    {
        if(!GroundCheck()) return;
        _rb.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
        _animController.JumpAnimation();
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

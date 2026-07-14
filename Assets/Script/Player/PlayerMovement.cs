using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーの動きを管理するクラス
/// ・移動処理
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Player"), SerializeField] private Player _player;
    [Header("PlayerInputHandler"), SerializeField] private PlayerInputHandler _playerInputHandler;
    [Header("CharacterAnimationController"), SerializeField] private CharacterAnimationController _animController;
    [Header("歩き速度"), SerializeField] private float _walkSpeed = 3f;
    [Header("走り速度"), SerializeField] private float _dashSpeed = 10f;
    [Header("ジャンプ速度"), SerializeField] private float _jumpSpeed = 5f;
    [Header("攻撃移動時の停止距離"), SerializeField] private float _attackMovementStopDistance = 1f;
    [Header("攻撃時の移動速度"), SerializeField] private float _attackMovementSpeed = 15f;
    [Header("滞空時間"), SerializeField] private float _hangTime = 1f;
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

    #region 攻撃関連の変数

    private Vector3 _attackTargetPosition;
    private bool _isAttackMoving;

    private float _hangTimer;

    #endregion

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

    private void FixedUpdate()
    {
        Move();
        HangTimerUpdate();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        if (!_player.PlayerState.CanMove)
        {
            if (_isAttackMoving) // 攻撃の踏み込みがある場合
            {
                AttackMove();
            }
            else
            {
                // 移動を停止する
                _rb.linearVelocity = new Vector3(
                    0,
                    _rb.linearVelocity.y,
                    0
                );
            }
            
            _moveBlendValue = 0;
            _animController.SetMoveAnimation(0, true);
            return;
        }
        
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
    /// 攻撃時の移動処理
    /// </summary>
    private void AttackMove()
    {
        // 攻撃対象まで一定距離以内だったら処理を行わない
        if (Vector3.Distance(transform.position, _attackTargetPosition) < _attackMovementStopDistance) return;

        _attackTargetPosition.y = transform.position.y;
        // 現在位置から攻撃対象位置まで移動
        transform.position = Vector3.MoveTowards(transform.position, _attackTargetPosition, 
            _attackMovementSpeed * Time.deltaTime);
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
    public bool GroundCheck()
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

    /// <summary>
    /// 攻撃対象に向かって移動を開始
    /// </summary>
    /// <param name="position">攻撃対象の位置</param>
    public void StartAttackMovement(Vector3 position)
    {
        _attackTargetPosition = position;
        _isAttackMoving = true;
    }

    public void StopAttackMovement()
    {
        _attackTargetPosition = Vector3.zero;
        _isAttackMoving = false;
    }

    private void HangTimerUpdate()
    {
        if (_rb.linearDamping != 0)
        {
            _hangTimer -= Time.deltaTime;
        }
    
        if (_hangTimer <= 0)
        {
            OnGravity();
        }
    }

    private void OnGravity()
    {
        if (_rb.linearDamping != 0)
        {
            _rb.linearDamping = 0;
            _hangTimer = _hangTime;
        }
    }

    private void OffGravity()
    {
        _rb.linearDamping = 40;
        _hangTimer = _hangTime;
    }

    /// <summary>
    /// 打ち上げ攻撃によるプレイヤーの打ち上げ
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    public void LaunchExecute(DamageContext context)
    {
        OffGravity();
        _rb.DOMoveY(context.HitReactionContext.UpPower, context.HitReactionContext.Duration);
    }
}

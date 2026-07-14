using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入力を受け取る
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの使用するInputSystem
    /// </summary>
    public PlayerInputSystem PlayerInputSystem { get; private set; }
    
    [Header("ダッシュになる判定時間"), SerializeField] private float _dashPressedTime;

    /// <summary>
    /// ジャンプアクション
    /// </summary>
    public Action JumpAction;
    /// <summary>
    /// 回避アクション
    /// </summary>
    public Func<UniTask> EvasionAction;
    
    /// <summary>
    /// プレイヤーの入力
    /// </summary>
    public Vector2 MoveInput { get; private set; }
    /// <summary>
    /// ダッシュの入力
    /// true：ダッシュした　false：ダッシュしていない
    /// </summary>
    public bool IsDashInput { get; private set; }

    private bool _isDashPressed; // ダッシュが押されている
    private float _dashPressTimer; // ダッシュになる判定時間のタイマー
    

    private void Awake()
    {
        PlayerInputSystem = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        PlayerInputSystem.Enable();
        PlayerInputSystem.Player.Move.performed += OnMove;
        PlayerInputSystem.Player.Move.canceled += OnMoveCancel;
        PlayerInputSystem.Player.Dash.performed += OnDash;
        PlayerInputSystem.Player.Dash.canceled += OnDashCancel;
        PlayerInputSystem.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        PlayerInputSystem.Disable();
    }

    private void Update()
    {
        DashTimerUpdate();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        _isDashPressed = true;
    }

    private void OnDashCancel(InputAction.CallbackContext context)
    {
        _isDashPressed = false;

        // ダッシュ判定時間以内なら、回避アクションを行う
        if (_dashPressTimer < _dashPressedTime)
        {
            EvasionAction?.Invoke();
        }

        _dashPressTimer = 0;
        IsDashInput = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        JumpAction?.Invoke();
    }

    /// <summary>
    /// ダッシュになる判定時間タイマーを更新する
    /// </summary>
    private void DashTimerUpdate()
    {
        if(!_isDashPressed) return;
        
        _dashPressTimer += Time.deltaTime;
        if (_dashPressTimer >= _dashPressedTime)
        {
            IsDashInput = true;
        }
    }
}

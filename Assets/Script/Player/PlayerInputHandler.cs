using System;
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

    public Action JumpAction;
    
    /// <summary>
    /// プレイヤーの入力
    /// </summary>
    public Vector2 MoveInput { get; private set; }
    /// <summary>
    /// ダッシュの入力
    /// true：ダッシュした　false：ダッシュしていない
    /// </summary>
    public bool IsDashInput { get; private set; }

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
        IsDashInput = true;
    }

    private void OnDashCancel(InputAction.CallbackContext context)
    {
        IsDashInput = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        JumpAction?.Invoke();
    }
}

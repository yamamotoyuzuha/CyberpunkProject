using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーが操作するキャラクター
/// </summary>
public class Player : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;
    
    [Header("CharacterSO")]
    [SerializeField] private CharacterSO _characterSo;

    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _playerInputSystem = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        _playerInputSystem.Enable();
        _playerInputSystem.Player.Move.performed += OnMove;
        _playerInputSystem.Player.Move.canceled += OnMoveCancel;
    }

    private void OnDisable()
    {
        _playerInputSystem.Disable();
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rb.linearVelocity = new Vector3(_moveDirection.x, 0, _moveDirection.y);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
    
    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        _moveDirection = Vector2.zero;
    }
}

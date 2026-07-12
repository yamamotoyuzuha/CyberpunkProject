using UnityEngine;

/// <summary>
/// プレイヤーが操作するキャラクター
/// </summary>
public class Player : CharacterBase
{
    [Header("CharacterSO"), SerializeField] private CharacterSO _characterSo;
    
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    private CharacterStatus _characterStatus;

    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    public PlayerState PlayerState { get; private set; }

    private void Awake()
    {
        // キャラクターのステータスを作成
        _characterStatus = new CharacterStatus(_characterSo.Hp, _characterSo.Stamina);
        PlayerState = new PlayerState();
    }
}

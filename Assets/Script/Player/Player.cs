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
    
    private void Awake()
    {
        // キャラクターのステータスを作成
        _characterStatus = new CharacterStatus(_characterSo.Hp, _characterSo.Stamina);
    }
}

using UnityEngine;

/// <summary>
/// キャラクターのSO
/// 共通のステータスなどを実装する
/// </summary>
public class CharacterSO : ScriptableObject
{
    [Header("HP")] [SerializeField] private int _hp;
    [Header("攻撃力")] [SerializeField] private int _attackPower;
    [Header("防御力")] [SerializeField] private int _defensePower;
    [Header("移動速度")] [SerializeField] private int _moveSpeed;

    #region プロパティ

    public int Hp => _hp;
    public int AttackPower => _attackPower;
    public int DefensePower => _defensePower;
    public int MoveSpeed => _moveSpeed;

    #endregion
}

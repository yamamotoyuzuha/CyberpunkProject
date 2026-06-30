using UnityEngine;

/// <summary>
/// キャラクターのSO
/// 共通のステータスなどを実装する
/// </summary>
[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [Header("HP")] [SerializeField] private int _hp;
    [Header("攻撃力")] [SerializeField] private int _attackPower;
    [Header("防御力")] [SerializeField] private int _defensePower;
    [Header("歩き移動速度")] [SerializeField] private float _walkSpeed;
    [Header("走り移動速度")] [SerializeField] private float _dashSpeed;

    #region プロパティ

    public int Hp => _hp;
    public int AttackPower => _attackPower;
    public int DefensePower => _defensePower;
    public float WalkSpeed => _walkSpeed;
    public float DashSpeed => _dashSpeed;

    #endregion
}

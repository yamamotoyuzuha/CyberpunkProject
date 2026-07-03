using UnityEngine;

/// <summary>
/// キャラクター共通のステータス
/// </summary>
public class CharacterStatus
{
    private int _hp;
    private int _stamina;

    #region プロパティ

    public int Hp
    {
        get => _hp;
        set => ReduceHp(value);
    }

    public int Stamina
    {
        get => _stamina;
        set => ReduceStamina(value);
    }

    #endregion
    
    public CharacterStatus(int hp, int stamina)
    {
        _hp = hp;
        _stamina = stamina;
    }

    /// <summary>
    /// HPを減らす
    /// </summary>
    /// <param name="damage">受けたダメージ</param>
    private void ReduceHp(int damage)
    {
        _hp = Mathf.Min(0, _hp - damage);
    }

    /// <summary>
    /// スタミナを減らす
    /// </summary>
    /// <param name="amount">消費したスタミナ</param>
    private void ReduceStamina(int amount)
    {
        _stamina = Mathf.Max(0, _stamina - amount);
    }
}

using System;
using UnityEngine;

/// <summary>
/// キャラクター共通のステータス
/// </summary>
public class CharacterStatus
{
    public int MaxHp { get; private set; }
    public int Hp { get; private set; }
    public int Stamina { get; private set; }

    #region イベント

    /// <summary>
    /// HPに変更があったときに呼ばれる
    /// <para>現在のHP、最大HP</para>
    /// </summary>
    public Action<int, int> OnHpChanged;

    #endregion

    public CharacterStatus(int hp, int stamina)
    {
        MaxHp = hp;
        Hp = hp;
        Stamina = stamina;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public void TakeDamage(int damage)
    {
        Debug.Log("攻撃前のHP" + Hp);
        Hp = Mathf.Max(0, Hp - damage);
        Debug.Log("攻撃後のHP" + Hp);
        OnHpChanged?.Invoke(Hp, MaxHp);
    }

    /// <summary>
    /// スタミナを減らす
    /// </summary>
    /// <param name="amount">消費したスタミナ</param>
    private void ReduceStamina(int amount)
    {
        Stamina = Mathf.Max(0, Stamina - amount);
    }
}

using System;
using Cysharp.Threading.Tasks;
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
    /// <summary>
    /// ダメージを表示する
    /// <para>ダメージ</para>
    /// </summary>
    public Func<Transform, int, UniTask> OnShowDamage;

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
    /// <param name="position">位置</param>
    /// <param name="damage">ダメージ</param>
    public void TakeDamage(Transform position, int damage)
    {
        Debug.Log("攻撃前のHP" + Hp);
        Hp = Mathf.Max(0, Hp - damage);
        Debug.Log("攻撃後のHP" + Hp);
        OnHpChanged?.Invoke(Hp, MaxHp);
        OnShowDamage?.Invoke(position, damage);
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

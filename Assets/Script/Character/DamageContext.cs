using UnityEngine;

/// <summary>
/// ダメージ情報を保持する
/// </summary>
public class DamageContext
{
    public HitReactionContext HitReactionContext { get; private set; }
    public int Damage { get; private set; }
    /// <summary>
    /// 攻撃を与えたプレイヤー
    /// </summary>
    public PlayerMovement PlayerAttacked { get; private set; }

    public DamageContext(HitReactionContext context, int damage, PlayerMovement player)
    {
        HitReactionContext = context;
        Damage = damage;
        PlayerAttacked = player;
    }
}

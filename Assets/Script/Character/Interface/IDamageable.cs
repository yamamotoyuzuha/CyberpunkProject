
/// <summary>
/// ダメージを受ける
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    void TakeDamage(DamageContext context);
}

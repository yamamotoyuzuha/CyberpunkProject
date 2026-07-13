using UnityEngine;

/// <summary>
/// Enemyの移動を管理するクラス
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ノックバックを実行
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    public void KnockbackExecute(DamageContext context)
    {
        Vector3 force =
            context.Direction.normalized * context.Power + Vector3.up * context.UpPower;
        _rb.AddForce(force, ForceMode.Impulse);
    }
}

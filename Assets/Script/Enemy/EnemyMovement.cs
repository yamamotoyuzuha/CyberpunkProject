using UnityEngine;
using DG.Tweening;

/// <summary>
/// Enemyの移動を管理するクラス
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("滞空時間"), SerializeField] private float _hangTime = 1f;
    
    private Rigidbody _rb;
    
    private float _hangTimer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HangTimerUpdate();
    }

    /// <summary>
    /// ノックバックを実行
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    public void KnockbackExecute(DamageContext context)
    {
        Vector3 force = context.HitReactionContext.Direction.normalized * context.HitReactionContext.Power 
                        + Vector3.up * context.HitReactionContext.UpPower;
        _rb.AddForce(force, ForceMode.Impulse);
    }
    
    private void HangTimerUpdate()
    {
        if (_rb.linearDamping != 0)
        {
            _hangTimer -= Time.deltaTime;
        }
    
        if (_hangTimer <= 0)
        {
            OnGravity();
        }
    }

    private void OnGravity()
    {
        if (_rb.linearDamping != 0)
        {
            _rb.linearDamping = 0;
            _hangTimer = _hangTime;
        }
    }

    private void OffGravity()
    {
        _rb.linearDamping = 40;
        _hangTimer = _hangTime;
    }

    /// <summary>
    /// 打ち上げを実行
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    public void LaunchExecute(DamageContext context)
    {
        OffGravity();
        _rb.DOMoveY(context.HitReactionContext.UpPower, context.HitReactionContext.Duration);
    }
}

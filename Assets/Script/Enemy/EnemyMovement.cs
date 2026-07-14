using UnityEngine;
using DG.Tweening;

/// <summary>
/// Enemyの移動を管理するクラス
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("滞空時間"), SerializeField] private float _hangTime = 1f;

    [Header("ヒットストップの設定")]
    [SerializeField] private float _amplitude = 0.05f; // 振れ幅
    [SerializeField] private int _vibrationFrequency = 10; // 振動回数
    
    private Rigidbody _rb;
    /// <summary>
    /// ヒットストップ用のシーケンス
    /// </summary>
    private Sequence _hitSequence;
    
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
    /// ダメージを受けたときに実行
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    public void DamageExecute(DamageContext context)
    {
        // 前回の演出が残ってたら、停止
        _hitSequence?.Kill();
        
        // ヒットストップが終了したら、ノックバックを実行
        HitStop(context).OnComplete(() => KnockbackExecute(context));
    }
    
    /// <summary>
    /// ヒットストップ
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    /// <returns>ヒットストップのシーケンス</returns>
    private Sequence HitStop(DamageContext context)
    {
        _hitSequence = DOTween.Sequence();
        // 振動させる
        _hitSequence.Append(transform.DOShakePosition(context.HitReactionContext.HitStopTime,
            _amplitude, _vibrationFrequency, fadeOut: false));
        return _hitSequence;
    }

    /// <summary>
    /// ノックバックを実行
    /// </summary>
    /// <param name="context">ダメージ情報</param>
    private void KnockbackExecute(DamageContext context)
    {
        // ノックバックする方向と威力を計算する
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

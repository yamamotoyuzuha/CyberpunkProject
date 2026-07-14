using TMPro;
using UnityEngine;

/// <summary>
/// Enemyの戦闘アクション
/// </summary>
public class EnemyCombat : MonoBehaviour
{
    
    // TODO：UIと処理を分ける
    
    [Header("攻撃時に表示するUI"), SerializeField] private GameObject _attackShowUI;
    [Header("攻撃時に表示するText"), SerializeField] private TextMeshProUGUI _attackTimerText;
    [Header("攻撃開始時間"), SerializeField] private float _attackStartTime;
    [Header("攻撃終了時間"), SerializeField] private float _attackEndTime;
    
    private PlayerMovement _target;
    private float _attackTimer;
    
    public bool IsAttacking {get; private set;}
    public bool IsAttackFinished {get; private set;}

    private void Update()
    {
        if (!IsAttacking) return;
        
        UpdateAttackUI();

        // 攻撃発生
        if (_attackTimer >= _attackStartTime)
        {
            Attack();
        }

        // 攻撃終了
        if (_attackTimer >= _attackEndTime)
        {
            AttackEnd();
        }
    }

    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void AttackStart(GameObject target)
    {
        _target = target.GetComponent<PlayerMovement>();
        
        _attackTimer = 0;
        IsAttacking = true;
        IsAttackFinished = false;
        
        _attackShowUI.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 攻撃UI更新
    /// </summary>
    private void UpdateAttackUI()
    {
        if (_attackTimerText == null) return;
        
        _attackTimer += Time.deltaTime;
        var remainTime = _attackStartTime - _attackTimer;
        _attackTimerText.text = Mathf.Max(remainTime, 0).ToString("F1");
    }

    private void Attack()
    {
        Debug.LogWarning("攻撃！");
        IsAttacking = false;
        
        // TODO：ここでプレイヤーが回避を取っているかを取得する
        if(_target == null) return;
        if (_target.IsEvasion)
        {
            Debug.LogWarning("回避成功");
            
            // スローモーション演出を行う
            // TODO：ここがマジックナンバーになってるからあとで変更を行う
            TimeManager.Instance.SlowMotion(new TimeData(0.4f, 0.8f));
        }
        else
        {
            Debug.LogWarning("回避失敗");
        }
    }

    private void AttackEnd()
    {
        _attackTimer = 0;
        IsAttacking = false;
        IsAttackFinished = true;
        _attackShowUI.gameObject.SetActive(false);
    }
}

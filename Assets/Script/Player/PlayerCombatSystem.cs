using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーの攻撃を管理するクラス
/// </summary>
public class PlayerCombatSystem : MonoBehaviour
{
    [Header("CharacterSO"), SerializeField] private CharacterSO _characterSo;
    [Header("Player"), SerializeField] private Player _player;
    [Header("プレイヤーの戦闘アクション"), SerializeField] private List<CombatActionBase> _combatActions;
    [Header("検出範囲の設定"), SerializeField] private Vector3 _detectionRange;

    private CombatPhaseManagement _combatPhaseManagement;
    private CombatActionPhase _actionPhase;
    
    private GameObject _closestEnemy; // 一番近いEnemy

    private void Awake()
    {
        _combatPhaseManagement = new CombatPhaseManagement(_combatActions);
    }
    
    private void Start()
    {
        
    }

    private void Update()
    {
        PlayerAttackInput();
    }

    private void PlayerAttackInput()
    {
        // TODO：これだと、処理が面倒だからプレイヤーはTickを行うだけで済むようにするのが理想
        // TODO：ここで各戦闘アクションが行えるか判定を行い、発動させる
        /*
        foreach (var action in _combatActions)
        {
            if (action.IsPlayerInputAction(_player.PlayerInputSystem))
            {
                var phase = CombatActionPhase.ActionPhase.Start;
                _actionPhase = new CombatActionPhase(phase, action);
            }
        }
        
        if(_actionPhase != null) _actionPhase.Action.PhaseUpdate(_actionPhase);
        */
        
        _combatPhaseManagement.TryAction(_player.PlayerInputSystem);
        _combatPhaseManagement.Tick();
    }

    /// <summary>
    /// 一番近い敵を取得する
    /// </summary>
    private void GetClosestEnemy()
    {
        // 自身の位置から検出範囲のコライダーを取得
        Collider[] enemys = Physics.OverlapBox(transform.position, _detectionRange, transform.rotation);
        if(enemys.Length == 0) return;

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var enemy in enemys)
        {
            // Enemy以外は判定を行わない
            if (!enemy.CompareTag("Enemy")) continue;
            
            // 自身から敵までの距離を求めて、一番近い敵を更新していく
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy.gameObject;
                closestDistance = distance;
            }
        }

        // 一番近い敵を設定
        _closestEnemy = closestEnemy;
    }

    /// <summary>
    /// 一番近い敵の方向へ向く
    /// </summary>
    private void DirectionClosestEnemy()
    {
        // 一番近い敵がいない場合、処理を行わない
        if(_closestEnemy == null) return;
        
        // 一番近い敵の方向を取得
        var direction = (_closestEnemy.transform.position - transform.position).normalized;
        direction.y = 0;
        _player.DesignatedDirectionRotation(direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 center = transform.position + Vector3.up * 1f;
        Vector3 size = _detectionRange;

        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size * 2f);
    }
}

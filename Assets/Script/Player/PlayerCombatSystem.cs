using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// プレイヤーの攻撃を管理するクラス
/// </summary>
public class PlayerCombatSystem : MonoBehaviour
{
    [Header("CharacterSO"), SerializeField] private CharacterSO _characterSo;
    [Header("Player"), SerializeField] private Player _player;
    [Header("プレイヤーの戦闘アクション"), SerializeField] private List<CombatActionBase> _combatActions;
    [Header("検出範囲の設定"), SerializeField] private Vector3 _detectionRange;

    private CombatExecutorManagement _combatExecutorManagement;
    private CombatSystemContext _combatSystemContext;
    
    private void Awake()
    {
        _combatExecutorManagement = new CombatExecutorManagement(_combatActions);
        _combatSystemContext = new CombatSystemContext(this, gameObject);
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
        _combatExecutorManagement.TryAction(_player.PlayerInputSystem, _combatSystemContext);
        _combatExecutorManagement.Tick();
    }
    
    /// <summary>
    /// ダメージを与える周辺のキャラクターを取得する
    /// </summary>
    /// <param name="count">攻撃対象人数</param>
    /// <returns>範囲内かつ、攻撃対象人数分のキャラクター</returns>
    public GameObject[] GetTakeDamageCharacter(int count)
    {
        // 自身の位置から検出範囲のコライダーを取得
        Collider[] enemys = Physics.OverlapBox(transform.position, _detectionRange, transform.rotation);
        if(enemys.Length == 0) return null;

        List<GameObject> damageable = new List<GameObject>();
        float closestDistance = 10f; // TODO：ここマジックナンバーになってるから変数に変更を行う
        foreach (var enemy in enemys)
        {
            // Enemy以外は判定を行わない
            if (!enemy.CompareTag("Enemy")) continue;
            
            // 自身から敵までの距離を求めて、一番近い敵を更新していく
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                damageable.Add(enemy.gameObject);
            }
            
            if(damageable.Count == count) break;
        }

        return damageable.ToArray();
    }

    /// <summary>
    /// 一番近い敵の方向へ向く
    /// </summary>
    /// <param name="character">ダメージを与えるキャラクター</param>
    public void DirectionClosestTakeDamageCharacter(GameObject[] character)
    {
        if(character.Length == 0) return;
        
        // 自身から敵までの距離を求めて、近い順にソートしていく
        var chara = character.ToList();
        chara.OrderBy(x => Vector3.Distance(transform.position, x.transform.position));

        // 一番近い敵の方向を取得
        var direction = (chara[0].transform.position - transform.position).normalized;
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

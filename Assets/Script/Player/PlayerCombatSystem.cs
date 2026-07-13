using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// プレイヤーの攻撃を管理するクラス
/// </summary>
public class PlayerCombatSystem : MonoBehaviour
{
    [Header("CharacterSO"), SerializeField] private CharacterSO _characterSo;
    [Header("PlayerInputHandler"), SerializeField] private PlayerInputHandler _inputHandler;
    [Header("PlayerMovement"), SerializeField] private PlayerMovement _playerMovement;
    [Header("プレイヤーの戦闘アクション"), SerializeField] private List<CombatActionBase> _combatActions;
    [Header("検出範囲の設定"), SerializeField] private Vector3 _detectionRange;
    [Header("検出視野角"), SerializeField] private float _viewAngle = 120f;
    [Header("検出距離"), SerializeField] private float _closestDistance = 10f;
    [Header("方向の重み（正面にいることを重視する）"), SerializeField] private float _directionWeight = 10f;
        
    private CombatExecutorManagement _combatExecutorManagement;
    private CombatSystemContext _combatSystemContext;
    
    private void Awake()
    {
        _combatExecutorManagement = new CombatExecutorManagement(_combatActions);
    }
    
    private void Start()
    {
        _combatSystemContext = new CombatSystemContext(this, GetComponent<Player>(),
            _inputHandler.PlayerInputSystem, GetComponent<ICharacterAnimator>());
    }

    private void Update()
    {
        PlayerAttackInput();
    }

    private void PlayerAttackInput()
    {
        _combatExecutorManagement.TryAction(_inputHandler.PlayerInputSystem, _combatSystemContext);
        _combatExecutorManagement.Tick();
    }
    
    /// <summary>
    /// 攻撃対象となる周辺のターゲットの取得
    /// </summary>
    /// <param name="count">攻撃対象人数</param>
    /// <returns>範囲内かつ、攻撃対象人数分のターゲット</returns>
    public GameObject[] GetAttackTarget(int count)
    {
        // 自身の位置から検出範囲のコライダーを取得
        Collider[] enemys = Physics.OverlapBox(transform.position, _detectionRange, transform.rotation);
        if(enemys.Length == 0) return null;

        List<GameObject> damageable = new List<GameObject>();
        foreach (var enemy in enemys)
        {
            // Enemy以外は判定を行わない
            if (!enemy.CompareTag("Enemy")) continue;
            
            // プレイヤーから敵までの距離
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            // 検出距離以内の場合のみ追加していく
            if (distance < _closestDistance)
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
    public void DirectionClosestEnemy(GameObject[] character)
    {
        if(character.Length == 0) return;

        var target = character.OrderByDescending(x =>
        {
            // 敵の方向
            var direction = (x.transform.position - transform.position).normalized;
            
            // プレイヤーの正面方向と敵の方向の内積を求める
            // 1に近いほど正面
            var dot = Vector3.Dot(transform.forward, direction);
            
            // プレイヤーから敵までの距離
            var distance = Vector3.Distance(transform.position, x.transform.position);

            // 正面に重みをつけて、優先しつつ距離も優先する
            return dot * _directionWeight - distance;
        }).First();

        // 一番近い敵の方向を取得
        var targetDirection = (target.transform.position - transform.position).normalized;
        targetDirection.y = 0;
        _playerMovement.DesignatedDirectionRotation(targetDirection);
        _playerMovement.StartAttackMovement(targetDirection);
    }
    
    /// <summary>
    /// 対象がプレイヤーの視野角内に存在するか判定する
    /// </summary>
    /// <param name="target">視野判定を行う対象</param>
    /// <returns>true：視野内　false：視野外</returns>
    private bool IsInView(Transform target)
    {
        // 対象の方向
        var direction = (target.position - transform.position).normalized;
        
        // プレイヤーの正面方向と対象の方向の内積を求める
        // 1に近いほど正面
        var dot = Vector3.Dot(transform.forward, direction);
        
        // 視野角の半分にして、cos値に変換
        // 120/2=60 cos(60度)=0.5
        var cosHalf = Mathf.Cos(_viewAngle / 2 * Mathf.Deg2Rad);
        
        // Dot値が基準値以上か判定を行う
        return dot >= cosHalf;
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

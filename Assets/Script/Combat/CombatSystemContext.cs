
/// <summary>
/// CombatActionExecutor（戦闘アクションの実行基底クラス）に対して
/// 必要な情報を提供するクラス
/// </summary>
public class CombatSystemContext
{
    /// <summary>
    /// 戦闘アクションを行ったキャラクター
    /// </summary>
    public Player Player { get; private set; }
    /// <summary>
    /// プレイヤーの戦闘システム
    /// </summary>
    public PlayerCombatSystem CombatSystem { get; private set; }
    /// <summary>
    /// プレイヤーの入力情報
    /// </summary>
    public PlayerInputSystem InputSystem { get; private set; }
    /// <summary>
    /// キャラクターのアニメーション制御を行うインターフェース
    /// </summary>
    public ICharacterAnimator CharacterAnimator { get; private set; }

    public CombatSystemContext(PlayerCombatSystem combatSystem, Player player, PlayerInputSystem inputSystem, ICharacterAnimator characterAnimator)
    {
        CombatSystem = combatSystem;
        Player = player;
        InputSystem = inputSystem;
        CharacterAnimator = characterAnimator;
    }
}

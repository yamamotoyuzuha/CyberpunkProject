public class CombatActionPhase
{
    /// <summary>
    /// アクションの実行フェーズ
    /// </summary>
    public enum ActionPhase
    {
        Active,
        Start,
        Update,
        End
    }

    public ActionPhase Phase;
    public readonly CombatActionBase Action;
    public CombatActionPhase(ActionPhase phase, CombatActionBase action)
    {
        Phase = phase;
        Action = action;
    }
}

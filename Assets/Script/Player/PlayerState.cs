/// <summary>
/// プレイヤーの状態を管理する
/// </summary>
public class PlayerState
{
    /// <summary>
    /// プレイヤーが動くことが可能か
    /// <para>true:可能　false：不可能</para>
    /// </summary>
    public bool CanMove { get; private set; } = true;

    /// <summary>
    /// プレイヤーの移動可能状態を設定する
    /// </summary>
    /// <param name="canMove">true：可能　false：不可能</param>
    public void SetCanMove(bool canMove)
    {
        CanMove = canMove;
    }
}

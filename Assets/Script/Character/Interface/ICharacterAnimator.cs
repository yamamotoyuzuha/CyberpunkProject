/// <summary>
/// キャラクターのアニメーションを制御するためのインターフェース
/// </summary>
public interface ICharacterAnimator
{
    /// <summary>
    /// 移動アニメーションを再生する
    /// </summary>
    void SetMoveAnimation(float moveParam, bool isFlag);
}

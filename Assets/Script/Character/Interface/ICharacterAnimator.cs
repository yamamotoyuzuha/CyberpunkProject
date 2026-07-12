/// <summary>
/// キャラクターのアニメーションを制御するためのインターフェース
/// </summary>
public interface ICharacterAnimator
{
    /// <summary>
    /// 移動アニメーションを再生する
    /// </summary>
    void SetMoveAnimation(float moveParam, bool isFlag);

    /// <summary>
    /// 攻撃アニメーションを再生する
    /// </summary>
    /// <param name="parameter">アニメーターのパラメータ名</param>
    /// <param name="actionInfo">攻撃アニメーションの情報</param>
    void PlayAttackAnimation(ActionAnimParameter parameter, ActionInfoBase actionInfo);

    /// <summary>
    /// Triggerで再生するアニメーションを再生する
    /// </summary>
    /// <param name="animationName">アニメーターのパラメータ名</param>
    void TriggerAnimation(string animationName);
}

using UnityEngine;

/// <summary>
/// キャラクターのアニメーションを行うクラス
/// </summary>
public class CharacterAnimationController : MonoBehaviour, ICharacterAnimator
{
    [Header("アニメーションのパラメータ名")]
    [Header("移動"), SerializeField] private string _moveAnimationName;
    [Header("ジャンプ"), SerializeField] private string _jumpParameter;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void SetMoveAnimation(float moveParam, bool isFlag)
    {
        _animator.SetFloat(_moveAnimationName, moveParam);
    }

    public void PlayAttackAnimation(ActionAnimParameter parameter, ActionInfoBase actionInfo)
    {
        _animator.SetFloat(parameter.ParameterFloatName, actionInfo.AnimationInfo.AnimParameter);
        _animator.SetTrigger(parameter.ParameterTriggerName);
    }

    public void TriggerAnimation(string animationName)
    {
        _animator.SetTrigger(animationName);
    }

    public bool IsAnimationPlaying(string animationTag)
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag(animationTag);
    }

    /// <summary>
    /// ジャンプアニメーション
    /// </summary>
    public void JumpAnimation()
    {
        _animator.SetTrigger(_jumpParameter);
    }
}

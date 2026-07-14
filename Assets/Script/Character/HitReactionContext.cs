using UnityEngine;

/// <summary>
/// 攻撃を受けた時の反応に使用するパラメータ
/// </summary>
public class HitReactionContext
{
    public ActionType ActionType { get; private set; }
    public Vector3 Direction { get; private set; }
    public float Power { get; private set; }
    public float UpPower { get; private set; }
    public float Duration { get; private set; }
    public float HitStopTime { get; private set; }

    public HitReactionContext(ActionType actionType, Vector3 direction, float power, float upPower, float duration, float hitStopTime)
    {
        ActionType = actionType;
        Direction = direction;
        Power = power;
        UpPower = upPower;
        Duration = duration;
        HitStopTime = hitStopTime;
    }
}

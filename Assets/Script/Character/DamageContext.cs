using UnityEngine;

/// <summary>
/// ダメージ情報を保持する
/// </summary>
public class DamageContext
{
    public bool IsKnockback { get; private set; }
    public Vector3 Direction { get; private set; }
    public float Power { get; private set; }
    public float UpPower { get; private set; }
    public float Duration { get; private set; }
    public int Damage { get; private set; }
    
    public DamageContext(bool isKnockback, Vector3 direction, float power, float upPower, float duration, int damage)
    {
        IsKnockback = isKnockback;
        Direction = direction;
        Power = power;
        UpPower = upPower;
        Duration = duration;
        Damage = damage;
    }
}

using UnityEngine;

/// <summary>
/// ゲーム内の時間を管理する
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    /// <summary>
    /// スローモーション中か
    /// <para>true：スローモーション中　false：スローモーション中じゃない</para>
    /// </summary>
    public bool IsSlowMotion { get; private set; }
    private TimeData _timeData;
    private float _slowMotionTimer;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        SlowMotionTimerUpdate();
    }
    
    private void SlowMotionTimerUpdate()
    {
        if (IsSlowMotion)
        {
            _slowMotionTimer += Time.deltaTime;
            if (_slowMotionTimer >= _timeData?.SlowMotionTime)
            {
                NormalMotion();
            }
        }
    }
    
    public void SlowMotion(TimeData timeData)
    {
        _timeData = timeData;
        Time.timeScale = _timeData.TimeScale;
        IsSlowMotion = true;
    }

    private void NormalMotion()
    {
        _timeData = null;
        Time.timeScale = 1f;
        IsSlowMotion = false;
        _slowMotionTimer = 0f;
    }
}

/// <summary>
/// スローモーションの設定データ
/// </summary>
public class TimeData
{
    public float TimeScale { get; private set; }
    public float SlowMotionTime { get; private set; }
    
    public TimeData(float timeScale, float slowMotionTime)
    {
        TimeScale = timeScale;
        SlowMotionTime = slowMotionTime;
    }
}

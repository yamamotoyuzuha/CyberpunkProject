using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// EnemyのUIを管理する
/// ・HPなど
/// </summary>
public class EnemyUI : MonoBehaviour
{
    [Header("HPUI"), SerializeField] private GameObject _hpUI;
    [Header("HPバー"), SerializeField] private Image _hpBar;
    
    private CharacterStatus _status;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="characterStatus">ステータス</param>
    public void Initialization(CharacterStatus characterStatus)
    {
        _status = characterStatus;
        _status.OnHpChanged += HpChange;
        HpChange(_status.Hp, _status.MaxHp);
    }

    private void LateUpdate()
    {
        // カメラと同じ向きに設定
        _hpBar.transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// Hpバーの変更
    /// </summary>
    /// <param name="value">現在のHP</param>
    /// <param name="maxValue">最大HP</param>
    private void HpChange(int value, int maxValue)
    {
        _hpBar.fillAmount = (float)value / maxValue;
    }
}

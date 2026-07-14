using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

/// <summary>
/// Enemyが受けたダメージを表示する
/// </summary>
public class EnemyDamageUI : MonoBehaviour
{
    public static EnemyDamageUI Instance {get; private set;}
    
    [Header("MainCamera"), SerializeField] private Camera _mainCamera;
    [Header("Canvas"), SerializeField] private Canvas _canvas;
    [Header("ダメージUIの生成場所"), SerializeField] private Transform _generationPosition;
    [Header("ダメージUIのPrefab"), SerializeField] private GameObject _damageUIPrefab;
    [Header("ダメージUIの表示する位置"), SerializeField] private Vector2 _displayPosition;
    [Header("ランダムの設定")] 
    [SerializeField] private float _randomMin = -60f;
    [SerializeField] private float _randomMax = 60f;
    [Header("UIの表示時間"), SerializeField] private int _displayTime;
    
    private readonly List<DamageUI> _damageUIs = new List<DamageUI>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GenerateDamageUI(5);
        }
    }

    public void Initialization(CharacterStatus status)
    {
        status.OnShowDamage += DamageUIShow;
    }

    private void Update()
    {
        DamageUIUpdate();
    }
    
    /// <summary>
    /// ダメージUIを表示する位置の更新を行う
    /// </summary>
    private void DamageUIUpdate()
    {
        // ダメージUIを表示後、表示するUIがあるのであればターゲットに追従するようにする
        foreach (var damageUI in _damageUIs)
        {
            // ダメージが非表示、UIを表示するターゲットがいない場合は処理を飛ばす
            if(!damageUI.DamageUIPrefabInstance.activeSelf) continue;
            if(damageUI.UITarget == null) continue;
            
            Vector3 offset = new Vector3(_displayPosition.x, _displayPosition.y, 0);
            Vector2 screenPos = damageUI.GetScreenLocalPosition(damageUI.UITarget.position + offset, _mainCamera);
            screenPos += damageUI.UIOffset;
            
            Vector2 localPos = damageUI.GetCanvasLocalPosition(screenPos, _canvas);
            damageUI.DamageUIPrefabInstance.transform.localPosition = localPos;
        }
    }
    
    /// <summary>
    /// UIの生成を行う
    /// </summary>
    /// <param name="count">生成する個数</param>
    private void GenerateDamageUI(int count)
    {
        // DamageUIを生成し、保持しておく
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(_damageUIPrefab, _generationPosition);
            var text = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var damageUI = new DamageUI(obj, text);
            
            _damageUIs.Add(damageUI);
            _damageUIs[i].DamageUIPrefabInstance.SetActive(false);
        }
    }

    private async UniTask DamageUIShow(Transform target, int damage)
    {
        // 表示中ではないDamageUIを取得
        DamageUI damageUI = null;
        foreach (var ui in _damageUIs)
        {
            if (!ui.DamageUIPrefabInstance.activeSelf)
            {
                damageUI = ui;
                break;
            }
        }
        
        // もし全てが表示中だった場合、UIを新しく生成する
        if (damageUI == null)
        {
            GenerateDamageUI(1);
        }
        else // 表示を行う
        {
            // ランダムで表示位置を決定
            Vector3 offset = new Vector3(_displayPosition.x, _displayPosition.y, 0);
            Vector2 screenPos = damageUI.GetScreenLocalPosition(target.position + offset, _mainCamera);
            // スクリーン座標が被らないようにランダムで値を作成
            Vector2 randomOffset = new Vector2(
                Random.Range(_randomMin, _randomMax), Random.Range(_randomMin, _randomMax)
            );
            screenPos += randomOffset;
            damageUI.SetTarget(target, randomOffset);
            
            Vector2 localPos = damageUI.GetCanvasLocalPosition(screenPos, _canvas);
            damageUI.DamageUIPrefabInstance.transform.localPosition = localPos;
            damageUI.DamageUIPrefabInstance.SetActive(true);
            
            damageUI.SetDamageText(damage);
            await damageUI.Hidden(_displayTime);
        }
    }
}

public class DamageUI
{
    /// <summary>
    /// ダメージUI
    /// </summary>
    public GameObject DamageUIPrefabInstance{get; private set;}
    /// <summary>
    /// ダメージUIを表示するターゲット
    /// </summary>
    public Transform UITarget {get; private set;}
    /// <summary>
    /// ダメージUIのオフセット
    /// </summary>
    public Vector2 UIOffset {get; private set;}
    
    private readonly TextMeshProUGUI _damageUIText;

    public DamageUI(GameObject obj, TextMeshProUGUI text)
    {
        DamageUIPrefabInstance = obj;
        _damageUIText = text;
    }
    
    public Vector2 GetCanvasLocalPosition(Vector3 screenPosition, Canvas canvas)
    {
        return canvas.transform.InverseTransformPoint(screenPosition);
    }
    
    /// <summary>
    /// ワールド座標をスクリーン座標に変換
    /// </summary>
    /// <param name="worldPosition">ワールド座標</param>
    /// <param name="mainCamera">メインカメラ</param>
    /// <returns></returns>
    public Vector2 GetScreenLocalPosition(Vector3 worldPosition, Camera mainCamera)
    {
        return RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);
    }
    
    /// <summary>
    /// ダメージをテキストに設定
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public void SetDamageText(int damage)
    {
        _damageUIText.text = damage.ToString();
    }
    
    /// <summary>
    /// ダメージUIを表示するターゲットの設定を行う
    /// </summary>
    /// <param name="target">ダメージUIを表示するターゲットの<c>Transform</c></param>
    /// <param name="offset">ダメージUIを表示する位置の<c>Vector2</c></param>
    public void SetTarget(Transform target, Vector2 offset)
    {
        UITarget = target;
        UIOffset = offset;
    }

    /// <summary>
    /// ダメージを表示したあと、指定時間後に非表示にする
    /// </summary>
    /// <param name="time">待機時間</param>
    public async UniTask Hidden(int time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));

        // DamageUIの初期化
        if (DamageUIPrefabInstance != null)
        {
            DamageUIPrefabInstance.SetActive(false);
            UITarget = null;
        }
    }
}

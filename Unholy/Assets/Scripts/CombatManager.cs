using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Component")]
    public static CombatManager _instance;

    [Header("Status")]
    [SerializeField] public static int _bossMaxHP = 2000;
    [SerializeField] public static int _playerMaxHP = 200;
    [SerializeField] public static float _playerMaxStamina = 100;

    public static float _currentBossHP { get; set; } = 0;
    public static float _currentPlayerHP;
    public static float _currentPlayerST;

    private static float _defenseTime = default;
    public static float _dist;
    public static float _attackRange = 3.5f;
    public static bool _isForward = false; // 앞 뒤 구분
    public static bool _parryingRange = false;
    public static bool _canParrying = false;
    public static bool _checkParrying = false;
    public static bool _longDefense = false;
    public static bool _isPlayerDead = false;
    public static bool _isIdle;
    public static bool _isBossDead { get; set; } = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeStats();

        _isIdle = false;
        _isBossDead = false;
        _isPlayerDead = false;
    }

    void Update()
    {
        ActivateParry();
    }

    /// <summary>
    /// 시작 시 설정된 최대 체력과 스태미너를 현재 체력과 스태미너로 설정하는 메소드입니다.
    /// </summary>
    public void InitializeStats()
    {
        _currentBossHP = _bossMaxHP;
        _currentPlayerHP = _playerMaxHP;
        _currentPlayerST = _playerMaxStamina;
    }

    void ActivateParry()
    {
        if (_parryingRange)
        {
            if (_canParrying == false)
            {
                _checkParrying = false;
                return;
            }
            else
            {
                _checkParrying = true;
                _canParrying = false;
            }
        }
    }

    // 플레이어와의 위치 계산
    public static void CheckDistance(Transform player, Transform boss)
    {
        _dist = Vector3.Distance(player.position, boss.position);
        Vector3 directionToPlayer = (player.position - boss.position).normalized;
        Vector3 bossForward = boss.forward;

        float dotProduct = Vector3.Dot(directionToPlayer, bossForward);

        if (dotProduct > 0.5f) _isForward = true;
        else _isForward = false;

        if (_isForward && _dist <= _attackRange) _parryingRange = true;
        else _parryingRange = false;
    }

    public static void CheckTime(bool state)
    {
        if (state) _defenseTime += Time.deltaTime;
        else _defenseTime = default;

        if (_defenseTime >= 5f) _longDefense = true;
        else _longDefense = false;
    }

    /// <summary>
    /// Player 또는 Monster가 받는 대미지를 체력에 반영하는 메소드입니다.
    /// </summary>
    /// <param name="type">대미지를 주는 주체를 의미합니다.</param>
    /// <param name="damage">받는 대미지량을 의미합니다.</param>
    public static void TakeDamage(string type, float damage)
    {
        if (type == "Player")
            _currentBossHP -= damage;
        if (type == "MinoAxe")
            _currentPlayerHP -= damage;
        else if (type == "MinoSlam")
            _currentPlayerHP -= damage;
    }

    /// <summary>
    /// Player의 Stamina를 소모시키는 메소드입니다.
    /// </summary>
    /// <param name="consumption">Stamina의 소모량을 의미합니다.</param>
    public static void ConsumeStamina(float consumption)
    {
        if(_currentPlayerST > 0)
        {
            _currentPlayerST -= consumption;
            _currentPlayerST = Mathf.Clamp(_currentPlayerST, 0, _playerMaxStamina);
        }
    }

    /// <summary>
    /// Player의 Stamina를 회복시키는 메소드입니다.
    /// </summary>
    /// <param name="recoveryQuantity">Stamina의 회복량을 의미합니다.</param>
    public static void RecoveryStamina(float recoveryQuantity)
    {
        if (_currentPlayerST < _playerMaxStamina)
        {
            _currentPlayerST += recoveryQuantity;
            _currentPlayerST = Mathf.Clamp(_currentPlayerST, 0, _playerMaxStamina);
        }
    }
}

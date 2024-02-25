using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CombatManager : MonoBehaviour
{
    [Header("Component")]
    public static CombatManager _instance;

    [Header("Status")]
    [SerializeField] private int _bossMaxHP = 2000;
    [SerializeField] private int _playerMaxHP = 200;
    [SerializeField] public static float _playerMaxStamina = 100;

    public static float _currentBossHP;
    public static float _currentPlayerHP;
    public static float _currentPlayerST;

    public static float _dist;
    public static float _attackRange = 6f;
    public static bool _isForward = false; // �� �� ����
    public static bool _parryingRange = false;
    public static bool _canParrying = false;
    public static bool _checkParrying = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeStats();
    }

    void Update()
    {
        ActivateParry();
    }

    /// <summary>
    /// ���� �� ������ �ִ� ü�°� ���¹̳ʸ� ���� ü�°� ���¹̳ʷ� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    public void InitializeStats()
    {
        _currentBossHP = _bossMaxHP;
        _currentPlayerHP = _playerMaxHP;
        _currentPlayerST = _playerMaxStamina;
    }

    // ���� ������ �Լ��Դϴ�.
    // ���� �Ŵ����� ���Ե� ��������, �и� ����� �����ϴ� �޼ҵ��Դϴ�.
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

    // �÷��̾���� ��ġ ���
    public static void CheckDistance(Transform player, Transform boss)
    {
        _dist = Vector3.Distance(player.position, boss.position);
        Vector3 directionToPlayer = (player.position - boss.position).normalized;
        Vector3 bossForward = boss.forward;

        float dotProduct = Vector3.Dot(directionToPlayer, bossForward);

        if (dotProduct > 0.5f)
        {
            _isForward = true;
        }
        else
        {
            _isForward = false;
        }

        if (_isForward && _dist <= _attackRange)
        {
            _parryingRange = true;
        }
        else
        {
            _parryingRange = false;
        }
    }

    /// <summary>
    /// Player �Ǵ� Monster�� �޴� ������� ü�¿� �ݿ��ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">������� �ִ� ��ü�� �ǹ��մϴ�.</param>
    /// <param name="damage">�޴� ��������� �ǹ��մϴ�.</param>
    public static void TakeDamage(string type, float damage)
    {
        if (type == "Player")
            _currentBossHP -= damage;
        else if (type == "Monster")
            _currentPlayerHP -= damage;

        // boss �� player�� ü���� Ȯ���ϱ� ���� �ӽ������� �����߽��ϴ�.
        Debug.Log($"_currentBossHP = {_currentBossHP}");
        Debug.Log($"_currentPlayerHP = {_currentPlayerHP}");
    }

    /// <summary>
    /// Player�� Stamina�� �Ҹ��Ű�� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="consumption">Stamina�� �Ҹ��� �ǹ��մϴ�.</param>
    public static void ConsumeStamina(float consumption)
    {
        if(_currentPlayerST > 0)
        {
            _currentPlayerST -= consumption;
            _currentPlayerST = Mathf.Clamp(_currentPlayerST, 0, _playerMaxStamina);
        }
    }

    /// <summary>
    /// Player�� Stamina�� ȸ����Ű�� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="recoveryQuantity">Stamina�� ȸ������ �ǹ��մϴ�.</param>
    public static void RecoveryStamina(float recoveryQuantity)
    {
        if (_currentPlayerST < _playerMaxStamina)
        {
            _currentPlayerST += recoveryQuantity;
            _currentPlayerST = Mathf.Clamp(_currentPlayerST, 0, _playerMaxStamina);
        }
    }
}

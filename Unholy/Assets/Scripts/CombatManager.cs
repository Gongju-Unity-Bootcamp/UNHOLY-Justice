using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Component")]
    public static CombatManager _instance;

    [Header("Status")]
    [SerializeField] private int _bossHP = 2000;
    [SerializeField] private int _playerHP = 200;
    [SerializeField] private static float _playerST = 100;
    private static float _currentBossHP;
    private static float _currentPlayerHP;
    public static float _currentPlayerST;

    [Header("Values")]
    private static float changeValue = 15f;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _currentBossHP = _bossHP;
        _currentPlayerHP = _playerHP;
        _currentPlayerST = _playerST;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void TakeDamage(string type, float damage)
    {
        if (type == "Player")
            _currentBossHP -= damage;
        else if (type == "Monster")
            _currentPlayerHP -= damage;

        Debug.Log(_currentBossHP);
        Debug.Log(_currentPlayerHP);
    }

    public static void DecreaseStamina()
    {
        if(_currentPlayerST > 0)
        {
            _currentPlayerST = Mathf.Clamp(_currentPlayerST, 0, _playerST);

            _currentPlayerST -= changeValue * Time.deltaTime;
            Debug.Log(_currentPlayerST);
        }
    }

    public static void IncreaseStamina()
    {
        if (_currentPlayerST < _playerST)
        {
            _currentPlayerST = Mathf.Clamp(_currentPlayerST, 0, _playerST);

            _currentPlayerST += changeValue * Time.deltaTime;
            Debug.Log(_currentPlayerST);
        }
    }
}

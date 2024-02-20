using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using UnityEngine;


public class MinoController : MonoBehaviour
{
    [SerializeField] private BehaviorTree _treeA;
    private bool _isNormal = true;
    public bool CanAttack { get; set; }

    public static bool ck = false;
    public bool _attackRange = false;
    public bool _backPOS = false;
    public bool _keepDEF = false;
    public bool _out7SEC = false;

    public float _bossHP = 10f;

    Animator _bossAnimator;

    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();

        _treeA = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("attackRange", () => _attackRange == true)
                    .Selector()
                        .Sequence()
                            .Condition("backPOS", () => _backPOS == true)
                            .CustomAction("BossBackstep")
                            .CustomAction("BossStompAttack")
                        .End()
                        .Sequence()
                            .Condition("keepDEF", () => _keepDEF == true)
                            .CustomAction("BossKickAttack")
                        .End()
                        .SelectorRandom()
                            .CustomAction("BossATK1") 
                            .CustomAction("BossATK2")
                            .CustomAction("BossATK3")
                            .CustomAction("BossATK4")
                        .End()
                    .End()
                .End()
                .Sequence()
                    .Condition("out7SEC", () => _out7SEC == true)
                    .CustomAction("BossJumpAttack")   
                .End()
            .End()
            .Build();
            
    }
    private void Update()
    {
        _treeA.Tick();
        //if (!_isNormal)
        //{
        //    ActivateAi();
        //}
    }

    /*void Parrying()
    {
        _treeA.Reset();
        _bossAnimator.Play("Parrying");
        _isNormal = false;
    }

    void ParryingEnd()
    {
        _isNormal = true;
    }*/

    // Idle을 EntryState로 둔다.
    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }

    // Collider에 의해 CanAttack의 값을 변경
    bool _isHit;

    IEnumerator ActivateAiCo()
    {
        while (true)
        {
            if (_isHit)
            {
                if (_bossHP <= 0)
                {
                    _bossAnimator.Play("BossDie");
                }
            }
            else
            {
                if (CanAttack)
                {
                    _treeA.Tick();
                }
                else
                {
                    _bossAnimator.Play("BossTrack");
                }
            }
            yield return null;
        }
    }

    public void IsStateEnd()
    {
        Debug.Log("End");
        ck = true;
    }
    public void IsStateStart()
    {
        Debug.Log("Start");
        ck = false;
    }
}
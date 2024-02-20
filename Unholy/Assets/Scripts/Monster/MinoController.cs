using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using UnityEngine;


public class MinoController : MonoBehaviour
{
    [SerializeField] private BehaviorTree _treeA;
    public bool _isIdle;   
    public bool _attackRange = false;
    public bool _backPOS = false;
    public bool _keepDEF = false;
    public bool _out7SEC = false;

    public float _bossHP = 10f;

    Animator _bossAnimator;
    Rigidbody _rigidbody;

    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

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
        if (_isIdle)
        {
            ActivateAi();
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    _isNormal = false;
        //    _treeA.RemoveActiveTask(_treeA.Root);
        //    _bossAnimator.Play("BossHit");
        //}
    }

    void Parrying()
    {
        _treeA.Reset();
        _bossAnimator.Play("BossHit");
        _isIdle = false;
    }

    void ParryingEnd()
    {
        _isIdle = true;
    }

    // Idle을 EntryState로 둔다.
    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }

    // Collider에 의해 _attackRange의 값을 변경
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
                if (_attackRange)
                {
                    _treeA.Tick(); // Attack Node Start.
                }
                else if (_out7SEC)
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
    public void ProcessBackstep()
    {
        _rigidbody.velocity = 1f * -_rigidbody.transform.forward;
        _rigidbody.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
    }
}
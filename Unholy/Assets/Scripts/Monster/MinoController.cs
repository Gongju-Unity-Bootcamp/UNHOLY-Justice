using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;


public class MinoController : MonoBehaviour
{
    [SerializeField] Vector3 _backStepPower = new Vector3(0f, 2.5f, 0f);
    [SerializeField] float _backstepSpeed = 5.5f;

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
                            .StateAction("BossBackstep", ProcessBackstep)
                            .StateAction("BossStompAttack")
                        .End()
                        .Sequence()
                            .Condition("keepDEF", () => _keepDEF == true)
                            .StateAction("BossKickAttack")
                        .End()
                        .SelectorRandom()
                            .StateAction("BossATK1")
                            .StateAction("BossATK2")
                            .StateAction("BossATK3")
                            .StateAction("BossATK4")
                        .End()
                    .End()
                .End()
                .Sequence()
                    .Condition("out7SEC", () => _out7SEC == true)
                    .StateAction("BossJumpAttack")
                .End()
                .Do("BossTrack", () =>
                {
                    _bossAnimator.Play("BossTrack");
                    return TaskStatus.Success;
                })
            .End()
            .Build();
    }

    private void OnEnable()
    {
        ActivateAi();
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
            if (_isIdle == false)
            {
                yield return null;
                continue;
            }

            if (_isHit)
            {
                if (_bossHP <= 0)
                {
                    _bossAnimator.Play("BossDie");
                }
            }
            else
            {
                _treeA.Tick();
            }
            yield return null;
        }
    }

    public void ProcessBackstep()
    {

        var localPower = transform.TransformVector(_backStepPower);
        _rigidbody.AddForce(localPower, ForceMode.Impulse);
        _rigidbody.velocity = _backstepSpeed * -_rigidbody.transform.forward;
    }
}
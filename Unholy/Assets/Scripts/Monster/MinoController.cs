using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class MinoController : MonoBehaviour
{
    [SerializeField] private BehaviorTree _treeA;
    [SerializeField] Transform _player;

    [SerializeField] Vector3 _backStepPower = new Vector3(0f, 2.5f, 0f);
    [SerializeField] float _backstepSpeed = 5.5f;

    [SerializeField] Vector3 _jumpPower = new Vector3(0f, 2.5f, 0f);
    [SerializeField] float _jumpSpeed = 5.5f;

    public bool _isIdle; // true 일시 Idle 상태 해제
    public bool _keepDEF = false;
    public bool _isForward = false; // 앞 뒤 구분

    public float _bossHP = 10f;
    public float _dist;
    public float _distance;
    public float _attackRange = 6f;
    public float _minJump = 15f;
    public float _maxJump = 20f;

    Animator _bossAnimator;
    Rigidbody _rigidbody;
    NavMeshAgent _agent;
    public float _meleeAttackRange = 4f;



    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        _treeA = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("attackRange", () => _dist <= _attackRange)
                    .Selector()
                        .Sequence()
                            .Condition("backPOS", () => _isForward == true)
                            .Do(() =>
                            {
                                _agent.isStopped = true;
                                return TaskStatus.Success;
                            })
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
                    .Condition("out7SEC", () => _dist >= _minJump && _dist < _maxJump)
                    .Do(() =>
                    {
                        _agent.isStopped = true;
                        return TaskStatus.Success;
                    })
                    .StateAction("BossJumpAttack", ProcessJumpAttack)
                .End()
                .RepeatUntilSuccess()
                    .Do("BossTrack", () =>
                    {
                        _bossAnimator.Play("BossTrack");
                        _agent.isStopped = false;
                        _agent.SetDestination(_player.position);
                        if (_dist > 5f)
                        {
                            return TaskStatus.Success;
                        }
                        else
                        {
                            _agent.isStopped = true;
                            return TaskStatus.Failure;
                        }
                    })
                .End()
            .End()
            .Build();
    }


    private void Update()
    {
        _checkDistance();
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
        transform.LookAt(_player.position);
        var localPower = transform.TransformVector(_backStepPower);
        _rigidbody.AddForce(localPower, ForceMode.Impulse);
        _rigidbody.velocity = _backstepSpeed * -_rigidbody.transform.forward;
    }

    public void ProcessJumpAttack()
    {
        transform.LookAt(_player.position);
        var localPower = transform.TransformVector(_jumpPower);
        _rigidbody.AddForce(localPower, ForceMode.Impulse);
        _rigidbody.velocity = _jumpSpeed * _rigidbody.transform.forward;
    }
    // 플레이어와의 위치 계산
    private void _checkDistance()
    {
        _dist = Vector3.Distance(_player.position, transform.position);
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        Vector3 bossForward = transform.forward;

        float dotProduct = Vector3.Dot(directionToPlayer, bossForward);

        if (dotProduct > 0.5f)
        {
            _isForward = false;
        }
        else
        {
            _isForward = true;
        }
    }
}
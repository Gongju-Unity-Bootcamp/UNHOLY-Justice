using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class MinoController : MonoBehaviour
{
    [SerializeField] private BehaviorTree _treeA;
    [SerializeField] Transform _player;

    public bool _isIdle; // true 일시 Idle 상태 해제
    public bool _keepDEF = false;
    public bool _isForward = false; // 앞 뒤 구분]
    public bool _parryingRange = false;

    public float _bossHP = 10f;
    float _dist;
    float _distance;
    public float _attackRange = 6f;
    public float _minJump = 15f;
    public float _maxJump = 20f;

    Animator _bossAnimator;
    NavMeshAgent _agent;



    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();
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
    private void Update()  { CheckDistance(); }
    private void OnEnable() { ActivateAi(); }

    // Collider에 의해 _attackRange의 값을 변경
    internal bool _isHit;
    IEnumerator ActivateAiCo()
    {
        while (true)
        {
            if (!_isIdle)
            {
                yield return null;
                continue;
            }

            if (_isHit)
            {
                //  보스 피깎이는거 여기
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
    
    internal bool _canParrying;
    public void Parrying()
    {
        if (_parryingRange)
        {
            if (_canParrying == false)
            {
                return;
            }
            else
            {
                _bossAnimator.Play("BossHit");
                _treeA.RemoveActiveTask(_treeA.Root);
                _canParrying = false;
            }
        }
    }

    // Idle을 EntryState로 둔다.
    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }


    public void ProcessBackstep()
    {
        transform.LookAt(_player.position);
    }

    public void ProcessJumpAttack()
    {
        transform.LookAt(_player.position);
    }

    // 플레이어와의 위치 계산
    public void CheckDistance()
    {
        _dist = Vector3.Distance(_player.position, transform.position);
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        Vector3 bossForward = transform.forward;

        float dotProduct = Vector3.Dot(directionToPlayer, bossForward);

        if (dotProduct > 0.5f)
        {
            _isForward = false;
        }
        else { _isForward = true; }

        if (dotProduct > 0.5f && _dist <= _attackRange)
        {
            _parryingRange = true;
        }
        else { _parryingRange = false; }
    }
}
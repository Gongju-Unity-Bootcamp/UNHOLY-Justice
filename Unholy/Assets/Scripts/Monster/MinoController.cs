using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MinoController : MonoBehaviour
{
    [SerializeField] internal BehaviorTree _treeA;
    [SerializeField] Transform _player;

    public static bool _specialAttack = false;
    internal bool _isHit;
    bool _look = false;

    public float _minJump = 10f;
    public float _maxJump = 12f;

    internal Animator _bossAnimator;
    public GameObject _slamPrefab;
    public GameObject _kickPrefab;

    public GameObject KickPosition;
    public GameObject SlamPosition;

    public ParticleSystem _trail;
    NavMeshAgent _agent;



    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _treeA = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("attackRange", () => CombatManager._dist <= CombatManager._attackRange)
                    .Selector()
                        .Sequence()
                            .Condition("backPOS", () => CombatManager._isForward == false)
                            .Do(() =>
                            {
                                _agent.isStopped = true;
                                return TaskStatus.Success;
                            })
                            .RandomChance(1, 3)
                            .StateAction("BossBackstep", () => 
                            { 
                                transform.LookAt(_player.position);
                                _bossAnimator.applyRootMotion = true;
                            })
                            .StateAction("BossStompAttack", () =>          
                            { 
                                _bossAnimator.applyRootMotion = false;
                                transform.LookAt(_player.position);
                            })
                        .End()
                        .Sequence()
                            .Condition("keepDEF", () => CombatManager._longDefense && CombatManager._dist <= CombatManager._attackRange)
                            .StateAction("BossKickAttack", () => { transform.LookAt(_player.position); })
                        .End()
                        .SelectorRandom()
                            .Sequence()
                                .StateAction("BossLook", () => { _look = true; })
                                .StateAction("BossATK1", () => { _look = false; })
                            .End()
                            .Sequence()
                                .StateAction("BossLook", () => { _look = true; })
                                .StateAction("BossATK2", () => { _look = false; })
                            .End()
                            .Sequence()
                                .StateAction("BossLook", () => { _look = true; })
                                .StateAction("BossATK3", () => { _look = false; })
                            .End()
                            .Sequence()
                                .StateAction("BossLook", () => { _look = true; })
                                .StateAction("BossATK4", () => { _look = false; })
                            .End()
                        .End()
                    .End()
                .End()
                .Sequence()
                    .Condition("out7SEC", () => CombatManager._dist >= _minJump && CombatManager._dist < _maxJump && CombatManager._isForward)
                    .Do(() =>
                    {
                        _agent.isStopped = true;
                        return TaskStatus.Success;
                    })
                    .StateAction("BossJumpAttack", () =>
                    {
                        transform.LookAt(_player.position);
                        _bossAnimator.applyRootMotion = true;
                    })
                    .Do(() => 
                    {
                        _bossAnimator.applyRootMotion = false;
                        return TaskStatus.Success;
                    }) 
                .End()
                .RepeatUntilSuccess()
                    .Do("BossTrack", () =>
                    {
                        _bossAnimator.Play("BossTrack");
                        _agent.isStopped = false;
                        _agent.SetDestination(_player.position);
                        if (CombatManager._dist > CombatManager._attackRange)
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
        CombatManager.CheckDistance(_player, gameObject.transform);
        if (_look)
        {
            ProcessLookAt();
        }


        if (CombatManager._checkParrying)
        {
            BossParrying();
        }

    }
    private void Start() { ActivateAi(); }

    IEnumerator ActivateAiCo()
    {
        while (true)
        {
            if (CombatManager._isIdle == false)
            {
                yield return null;
                continue;
            }

            if (CombatManager._currentBossHP <= 0)
            {
                _bossAnimator.Play("BossDie");
                CombatManager._isBossDead = true;
            }
            else
            {
                _treeA.Tick();
            }
            yield return null;
        }
    }

    public void BreakDefense()
    {
        if (CombatManager._isForward && CombatManager._dist <= CombatManager._attackRange)
        {
            transform.LookAt(_player.position);
            _player.GetComponent<PlayerController>().isDamage = true;
            CombatManager.ConsumeStamina(CombatManager._currentPlayerST);
        }
    }

    void BossParrying()
    {
        _bossAnimator.Play("BossHit");
        _treeA.RemoveActiveTask(_treeA.Root);
    }

    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }


    public void ProcessLookAt()
    {
        Vector3 targetDirection = (_player.position - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7.5f * Time.deltaTime);
        }
    }

    public void GroundSlam()
    {
        Vector3 _slamPosition = SlamPosition.transform.position;
        _slamPosition.y = 0.05f;

        Instantiate(_slamPrefab, _slamPosition, Quaternion.identity);
        _specialAttack = true;
    }

    public void GroundKick()
    {
        Vector3 _kickPosition = KickPosition.transform.position;
        _kickPosition.y = 0.05f;

        Instantiate(_kickPrefab, _kickPosition, transform.rotation);
        _specialAttack = true;
    }

    public void ActiveTrail(float _time)
    {
        var _tempTrail = _trail.main;
        
        _tempTrail.duration = default;

        _tempTrail.duration = _time;
        
        _trail.Play();
    }

    public void BossSound(string name)
    {
        SoundManager.Instance.Play(SoundType.Effect, name);
    }
}
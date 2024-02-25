using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro.EditorUtilities;

public class MinoController : MonoBehaviour
{
    [SerializeField] internal BehaviorTree _treeA;
    [SerializeField] Transform _player;

    internal bool _isHit;
    public bool _isIdle; // true 일시 Idle 상태 해제
    public bool _keepDEF = false;

    public float _bossHP = 10f;
    public float _minJump = 10f;
    public float _maxJump = 12f;

    internal Animator _bossAnimator;
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
                            .StateAction("BossBackstep", ProcessBackstep)
                            .StateAction("BossStompAttack")
                        .End()
                        .Sequence()
                            .Condition("keepDEF", () => _keepDEF == true)
                            .StateAction("BossKickAttack")
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
                    .StateAction("BossJumpAttack", ProcessLookAt)
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
    bool _look = false;
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
    private void OnEnable() { ActivateAi(); }

    IEnumerator ActivateAiCo()
    {
        while (true)
        {
            if (!_isIdle)
            {
                yield return null;
                continue;
            }

            if (_bossHP <= 0)
            {
                _bossAnimator.Play("BossDie");
            }
            else
            {
                _treeA.Tick();
            }
            yield return null;
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

    public void ProcessBackstep()
    {
        transform.LookAt(_player.position);
    }

    public void ProcessJumpAttack()
    {
        transform.LookAt(_player.position);
    }
}
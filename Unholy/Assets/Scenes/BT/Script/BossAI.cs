using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class BossAi : MonoBehaviour
{
    [SerializeField] private BehaviorTree _treeA;
    [SerializeField] private bool _isNotPlayerInBossroom = true;
    [SerializeField] private float _bossHP = 10f;
    [SerializeField] private bool _parrying = false;
    [SerializeField] private bool _attackRange = false;
    [SerializeField] private bool _turnBack = false;
    [SerializeField] private bool _playerDefense = false;
    [SerializeField] private bool _outRange = false;

    Animator _bossAnimator;

    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();

        _treeA = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence("2 Sequence")
                        .Condition("Player isn't in Bossroom", IsPlayerInBossroom)
                        .Do("BossIdle", BossIdle)
                    .End()
                .Selector()
                    .Sequence("1 Sequence")
                        .Condition("BossHp <= 0", () => _bossHP <= 0)
                        .Do("Bossdie", BossDie)
                    .End()
                    .Sequence("1 Sequence")
                        .Condition("Player Parrying", IsPlayerParrying)
                        .Do("BossHit", BossHit)
                    .End()
                .Selector()
                    .Sequence("1 Sequence")
                        .Condition("Player is in Attack Range", IsPlayerInAttackRange)
                        .Selector()
                            .Sequence("1 Sequence")
                                .Condition("Player located back", IsPlayerLocatedBack)
                              .Do("BackJump", BossBackstep)
                              .Do("Stump Attack", BossStompAttack)
                            .End()
                            .Sequence("1 Sequence")
                                .Condition("Player keep defense more than 5sec", IsPlayerKeepDefense)
                              .Do("Kick Attck", BossKickAttack)
                            .End()
                            .SelectorRandom("Random Attack")
                                .Do("Attck1", BossNormalAttack1)
                                .Do("Attck2", BossNormalAttack2)
                                .Do("Attck3", BossComboAttack1)
                                .Do("Attck4", BossComboAttack2)
                            .End()
                        .End()
                    .End()
                    .Sequence("1 Sequence")
                        .Condition("Player out of attack range", IsPlayerOutOfRange)
                        .Do("Jump Attack", BossJumpAttack)
                    .End()
                    //.Sequence("1 Sequence")
                    //  .Condition("Boss Track Player", () => _trackRange)
                    .Do("Boss Track", BossTrack)
                    //.End()
                .End()
            .End()
            .Build();
    }

    private void Update()
    {
        // Update our tree every frame
        _treeA.Tick();
    }

    private bool IsPlayerInBossroom()
    {
        if (_isNotPlayerInBossroom == true) {  return true; }
        else { return false; }
    }

    private bool IsPlayerParrying()
    {
        if (_parrying == true) { return true; }
        else { return false; }
    }

    private bool IsPlayerInAttackRange()
    {
        if (_attackRange == true) { return true; }
        else { return false; }
    }

    private bool IsPlayerLocatedBack()
    {
        if (_turnBack == true) 
        {
            _turnBack = false;
            return true; 
        }
        else { return false; }
    }

    private bool IsPlayerKeepDefense()
    {
        if (_playerDefense == true) { return true; }
        else { return false; }
    }
    private bool IsPlayerOutOfRange()
    {
        if (_outRange == true) { return true; }
        else { return false; }
    }


    private TaskStatus BossIdle()
    {
        _bossAnimator.Play("BossIdle");
        return TaskStatus.Success;
    }
    private TaskStatus BossDie()
    {
        _bossAnimator.Play("BossDie");
        return TaskStatus.Success;
    }

    private TaskStatus BossHit()
    {
        _bossAnimator.Play("BossHit");
        return TaskStatus.Success;
    }

    private TaskStatus BossTrack()
    {
        _bossAnimator.Play("BossTrack");
        return TaskStatus.Success;
    }

    private TaskStatus BossKickAttack() 
    {
        _bossAnimator.Play("BossKickAttack");
        return TaskStatus.Success;
    }

    private TaskStatus BossBackstep()
    {
        _bossAnimator.Play("BossBackstep");
        return TaskStatus.Success;
    }

    private TaskStatus BossStompAttack()
    {
        _bossAnimator.Play("BossStompAttack");
        return TaskStatus.Success;
    }

    private TaskStatus BossNormalAttack1()
    {
        _bossAnimator.Play("BossNormalAttack1");
        return TaskStatus.Success;
    }
    private TaskStatus BossNormalAttack2()
    {
        _bossAnimator.Play("BossNormalAttack2");
        return TaskStatus.Success;
    }
    private TaskStatus BossComboAttack1()
    {
        _bossAnimator.Play("BossCombo1");
        return TaskStatus.Success;
    }
    private TaskStatus BossComboAttack2()
    {
        _bossAnimator.Play("BossCombo2");
        return TaskStatus.Success;
    }
    private TaskStatus BossJumpAttack()
    {
        _bossAnimator.Play("BossJumpAttack");
        return TaskStatus.Success;
    }
}

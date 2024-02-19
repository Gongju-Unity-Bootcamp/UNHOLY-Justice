using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class BossAi : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree _treeA;
    [SerializeField]
    private bool _notGround = true;
    [SerializeField]
    private bool _bossDie = false;
    [SerializeField]
    private bool _playerGard = false;
    [SerializeField]
    private bool _attackRange = false;
    [SerializeField]
    private bool _trackRange = true;
    [SerializeField]
    private bool _turnBack = false;
    [SerializeField]
    private bool _playerDefense = false;

    private void Awake()
    {
        _treeA = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence("2 Sequence")
                        .Condition("Player Not Ground", () => _notGround)
                        .Do("BossIdle", () => TaskStatus.Success)
                    .End()
                .Selector()
                    .Sequence("1 Sequence")
                        .Condition("BossHp == 0", () => _bossDie)
                        .Do("Bossdie", () => TaskStatus.Success)
                    .End()
                    .Sequence("1 Sequence")
                        .Condition("Player Gard", () => _playerGard)
                        .Do("BossHit", () => TaskStatus.Success)
                    .End()
                .Selector()
                    .Sequence("1 Sequence")
                        .Condition("Player in Attack", () => _attackRange)
                        .Selector()
                            .Sequence("1 Sequence")
                                .Condition("Player turn Boss Back", () => _turnBack)
                              .Do("BackJump", () => TaskStatus.Success)
                              .Do("Stumping", () => TaskStatus.Success)
                            .End()
                            .Sequence("1 Sequence")
                                .Condition("player Defense over Time", () => _playerDefense)
                              .Do("Kick Attck", () => TaskStatus.Success)
                            .End()
                            .SelectorRandom("Random Selsctor")
                                .Do("Attck1", () => TaskStatus.Success)
                                .Do("Attck2", () => TaskStatus.Success)
                                .Do("Attck3", () => TaskStatus.Success)
                                .Do("Attck4", () => TaskStatus.Success)
                            .End()
                        .End()
                    .End()
                        //.Sequence("1 Sequence")
                        //  .Condition("Boss Track Player", () => _trackRange)
                        .Do("Boss Track", () => TaskStatus.Success)
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
}

using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System;
using System.Collections;

public class MinoController : MonoBehaviour
{
    [SerializeField] private BehaviorTree _treeA;

    public bool _attackRange = false;
    public bool _backPOS = false;
    public bool _keepDEF = false;
    public bool _out7SEC = false;

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
                            .Do(() => TaskStatus.Success)
                            .Do(() => TaskStatus.Success)
                        .End()
                        .Sequence()
                            .Condition("keepDEF", () => _keepDEF == true)
                            .Do(() => TaskStatus.Success)
                        .End()
                        .SelectorRandom()
                            .Do(() => TaskStatus.Success)
                            .Do(() => TaskStatus.Success)
                            .Do(() => TaskStatus.Success)
                            .Do(() => TaskStatus.Success)
                        .End()
                    .End()
                .End()
                .Sequence()
                    .Condition("out7SEC", () => _out7SEC == true)
                    .Do(() => TaskStatus.Success)   
                .End()
            .End()
            .Build();
            
    }

    private void Update()
    {
        _treeA.Tick();
    }

}
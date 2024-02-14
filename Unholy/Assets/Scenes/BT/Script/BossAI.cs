using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float bossHP = 10f;
    Animator _bossAnimator;
    BehaviorTreeRunner _btRunner;
    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();
        _btRunner = new BehaviorTreeRunner(SettingBT());
    }
    void Start()
    {

    }

    void Update()
    {
        _btRunner.Operate();
    }
    INode SettingBT()
    {
        return new SelectorNode
        (
            new List<INode>()
            {
            new ConditionNode(() => bossHP <= 0, new ActionNode(BossDie)),
            new ActionNode(BossIdle)
            }

        );
    }

    INode.ENodeState BossIdle()
    {
        _bossAnimator.Play("BossIdle");
        return INode.ENodeState.Success;
        }

    INode.ENodeState BossDie()
    {
        _bossAnimator.Play("BossDie");
        return INode.ENodeState.Success;
    }


}


using System.Collections.Generic;
using System;

// INode 인터페이스
public interface INode
{
    public enum ENodeState
    {
        Running,
        Success,
        Failure,
    }
    public ENodeState Evaluate();
}

// Action Node 클래스
public sealed class ActionNode : INode
{
    Func<INode.ENodeState> _onUpdate = null;

    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }
    public INode.ENodeState Evaluate() => _onUpdate.Invoke();
}

// Selector Node 클래스
public sealed class SelectorNode : INode
{
    List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if (_childs == null)
        {
            return INode.ENodeState.Failure;
        }

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;
                case INode.ENodeState.Success:
                    return INode.ENodeState.Success;
            }
        }

        return INode.ENodeState.Failure;
    }
}

// Condition Node 클래스
public sealed class ConditionNode : INode
{
    INode _onSuccessNode;
    Func<bool> _condition;

    public ConditionNode(Func<bool> condition, INode onSuccessNode)
    {
        _condition = condition;
        _onSuccessNode = onSuccessNode;
    }

    public INode.ENodeState Evaluate()
    {
        if (_condition.Invoke())
        {
            return _onSuccessNode.Evaluate();
        }
        else
        {
            return INode.ENodeState.Failure;
        }
    }
}

// BT를 실행하기 위한 BehaviorTreeRunner 클래스
public class BehaviorTreeRunner
{
    INode _rootNode;
    public BehaviorTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}

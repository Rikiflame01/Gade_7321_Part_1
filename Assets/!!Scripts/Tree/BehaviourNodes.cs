using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Node
public abstract class BTNode
{
    public abstract bool Invoke();
}

// Selector Node
public class Selector : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public Selector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override bool Invoke()
    {
        foreach (var node in nodes)
        {
            if (node.Invoke())
            {
                return true;
            }
        }
        return false;
    }
}

// Sequence Node
public class Sequence : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public Sequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override bool Invoke()
    {
        foreach (var node in nodes)
        {
            if (!node.Invoke())
            {
                return false;
            }
        }
        return true;
    }
}

// Action Node
public class ActionNode : BTNode
{
    public delegate bool ActionNodeDelegate();
    private readonly ActionNodeDelegate action;

    public ActionNode(ActionNodeDelegate action)
    {
        this.action = action;
    }

    public override bool Invoke()
    {
        return action();
    }
}

// Condition Node
public class ConditionNode : BTNode
{
    public delegate bool ConditionNodeDelegate();
    private readonly ConditionNodeDelegate condition;

    public ConditionNode(ConditionNodeDelegate condition)
    {
        this.condition = condition;
    }

    public override bool Invoke()
    {
        return condition();
    }
}

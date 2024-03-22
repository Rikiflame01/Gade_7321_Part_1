using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Node
public abstract class BTNode
{
    public abstract bool Evaluate();
}

// Selector Node
public class Selector : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public Selector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override bool Evaluate() 
    {
        foreach (var node in nodes)
        {
            if (node.Evaluate()) 
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

    public override bool Evaluate()
    {
        foreach (var node in nodes)
        {
            if (!node.Evaluate())
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

    public override bool Evaluate()
    {
        return action();
    }
}

// Condition Node
public class DecoratorNode : BTNode
{
    public delegate bool ConditionNodeDelegate();
    private readonly ConditionNodeDelegate condition;

    public DecoratorNode(ConditionNodeDelegate condition)
    {
        this.condition = condition;
    }

    public override bool Evaluate()
    {
        return condition();
    }
}

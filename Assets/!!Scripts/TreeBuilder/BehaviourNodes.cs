using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this class is used to manage the behaviour nodes in the game. 
 * * * It is used to create the nodes for the behaviour tree, 
 * * * evaluate the nodes, and create the different types of nodes.
 * * * */

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

// Decorator Node
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

// Repeat Until Fail Node
public class RepeatUntilFail : BTNode
{
    private BTNode node;

    public RepeatUntilFail(BTNode node)
    {
        this.node = node;
    }

    public override bool Evaluate()
    {
        while (true)
        {
            bool success = node.Evaluate();
            if (!success)
            {
                // Stops repeating when the child node fails
                return false;
            }
        }
    }
}

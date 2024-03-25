using System.Collections.Generic;

/* 
 * * * This class is used to build the behavior tree for the AI.
 * * * It is used to create the selector and sequence nodes for the behavior tree.
 * * */

public class AIBehaviorTreeBuilder
{
    private BTNode root;

    public AIBehaviorTreeBuilder Selector(List<BTNode> nodes)
    {
        root = new Selector(nodes);
        return this;
    }

    public AIBehaviorTreeBuilder Sequence(List<BTNode> nodes)
    {
        root = new Sequence(nodes);
        return this;
    }

    public BTNode Build()
    {
        return root;
    }
}

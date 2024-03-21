using System.Collections.Generic;

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

using Godot;

public partial class BTSelector : BTNode
{
    private int currentChildIndex = 0;

    public override BTState Tick(float delta)
    {
        for (int i = currentChildIndex; i < GetChildCount(); i++)
        {
            var child = GetChild<BTNode>(i);
            var state = child.Tick(delta);

            if (state == BTState.Running)
            {
                currentChildIndex = i;
                return BTState.Running;
            }
            else if (state == BTState.Success)
            {
                Reset();
                return BTState.Success;
            }
        }

        Reset();
        return BTState.Failure;
    }

    public override void Reset()
    {
        currentChildIndex = 0;
        foreach (var child in GetChildren())
        {
            if (child is BTNode btNode)
                btNode.Reset();
        }
    }
}

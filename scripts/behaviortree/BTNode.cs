using Godot;

public enum BTState
{
    Running,
    Success,
    Failure
}

public abstract partial class BTNode : Node
{
    public abstract BTState Tick(float delta);
    
    public virtual void Reset() { }
}
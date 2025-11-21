using Godot;

public partial class BTAreAlliesAvailable : BTNode
{
    [Export] public int MaxAllies = 2;
    
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null) return BTState.Failure;
        
        return enemy.CurrentAllyCount < MaxAllies ? BTState.Success : BTState.Failure;
    }
}
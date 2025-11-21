using Godot;

public partial class BTCanAttack : BTNode
{
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null) return BTState.Failure;
        
        return enemy.CanAttack() ? BTState.Success : BTState.Failure;
    }
}
using Godot;

public partial class BTFlee : BTNode
{
    [Export] public float FleeSpeed = 150.0f;
    [Export] public float SafeDistance = 300.0f;
    
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null || enemy.Player == null) 
            return BTState.Failure;
        
        float distance = enemy.GlobalPosition.DistanceTo(enemy.Player.GlobalPosition);
        
        if (distance >= SafeDistance)
        {
            enemy.SetState("Idle");
            return BTState.Success;
        }
        
        Vector2 direction = (enemy.GlobalPosition - enemy.Player.GlobalPosition).Normalized();
        enemy.Velocity = direction * FleeSpeed;
        enemy.MoveAndSlide();
        
        enemy.SetState("Fleeing");
        return BTState.Running;
    }
}
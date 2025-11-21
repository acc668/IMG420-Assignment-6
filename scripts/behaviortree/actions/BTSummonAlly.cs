using Godot;

public partial class BTSummonAlly : BTNode
{
    [Export] public float SummonCooldown = 5.0f;
    [Export] public float SpawnDistance = 100.0f;
    
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null) return BTState.Failure;
        
        enemy.SummonAlly(SpawnDistance);
        enemy.SetState("Summoning");
        
        return BTState.Success;
    }
}
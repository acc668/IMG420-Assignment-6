using Godot;

public partial class BTIsHealthCritical : BTNode
{
    [Export] public float CriticalThreshold = 20.0f;
    
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null) return BTState.Failure;
        
        float healthPercent = (enemy.CurrentHealth / enemy.MaxHealth) * 100;
        return healthPercent < CriticalThreshold ? BTState.Success : BTState.Failure;
    }
}
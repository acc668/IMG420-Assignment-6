using Godot;

public partial class BTIsHealthLow : BTNode
{
    [Export] public float LowHealthThreshold = 50.0f;
    
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null) return BTState.Failure;
        
        float healthPercent = (enemy.CurrentHealth / enemy.MaxHealth) * 100;
        return healthPercent < LowHealthThreshold ? BTState.Success : BTState.Failure;
    }
}
using Godot;

public partial class BTAttack : BTNode
{
    [Export] public float AttackDamage = 10.0f;
    
    private Enemy enemy;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null || enemy.Player == null) 
            return BTState.Failure;
        
        enemy.Attack(AttackDamage);
        enemy.SetState("Attacking");
        
        return BTState.Success;
    }
}

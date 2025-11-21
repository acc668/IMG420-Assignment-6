using Godot;

public partial class BTIsPlayerInRange : BTNode
{
	[Export] public float DetectionRange = 200.0f;
	[Export] public float AttackRange = 50.0f;
	[Export] public bool CheckAttackRange = false;
	
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
		float rangeToCheck = CheckAttackRange ? AttackRange : DetectionRange;
		
		return distance <= rangeToCheck ? BTState.Success : BTState.Failure;
	}
}

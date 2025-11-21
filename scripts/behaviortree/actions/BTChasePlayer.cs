using Godot;

public partial class BTChasePlayer : BTNode
{
	[Export] public float ChaseSpeed = 100.0f;
	[Export] public float StoppingDistance = 60.0f; // Stop this far from player
	
	private Enemy enemy;

	public override void _Ready()
	{
		enemy = GetOwner<Enemy>();
	}

	public override BTState Tick(float delta)
	{
		if (enemy == null || enemy.Player == null) 
			return BTState.Failure;
		
		float distanceToPlayer = enemy.GlobalPosition.DistanceTo(enemy.Player.GlobalPosition);
		
		// Stop moving if close enough to player
		if (distanceToPlayer <= StoppingDistance)
		{
			enemy.Velocity = Vector2.Zero;
			enemy.SetState("In Range");
			return BTState.Success; // Close enough, stop chasing
		}
		
		// Chase the player
		Vector2 direction = (enemy.Player.GlobalPosition - enemy.GlobalPosition).Normalized();
		enemy.Velocity = direction * ChaseSpeed;
		enemy.MoveAndSlide();
		
		enemy.SetState("Chasing");
		return BTState.Running;
	}
}

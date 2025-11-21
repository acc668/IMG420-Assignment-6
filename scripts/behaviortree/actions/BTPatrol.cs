using Godot;

public partial class BTPatrol : BTNode
{
    [Export] public float PatrolSpeed = 50.0f;
    [Export] public float WaypointThreshold = 10.0f;
    
    private Enemy enemy;
    private int currentWaypointIndex = 0;

    public override void _Ready()
    {
        enemy = GetOwner<Enemy>();
    }

    public override BTState Tick(float delta)
    {
        if (enemy == null || enemy.PatrolPoints.Count == 0) 
            return BTState.Failure;
        
        Vector2 targetPoint = enemy.PatrolPoints[currentWaypointIndex];
        float distance = enemy.GlobalPosition.DistanceTo(targetPoint);
        
        if (distance < WaypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % enemy.PatrolPoints.Count;
            return BTState.Success;
        }
        
        Vector2 direction = (targetPoint - enemy.GlobalPosition).Normalized();
        enemy.Velocity = direction * PatrolSpeed;
        enemy.MoveAndSlide();
        
        enemy.SetState("Patrolling");
        return BTState.Running;
    }

    public override void Reset()
    {
        currentWaypointIndex = 0;
    }
}
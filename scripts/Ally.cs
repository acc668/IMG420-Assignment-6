using Godot;

public partial class Ally : CharacterBody2D
{
	[Export] public float MaxHealth = 50.0f;
	[Export] public float Speed = 80.0f;
	
	private float currentHealth;
	private HealthBar healthBar;
	private Enemy parentEnemy;
	private Player player;
	private AnimatedSprite2D sprite;
	private string currentAnimation = "";

	public override void _Ready()
	{
		currentHealth = MaxHealth;
		healthBar = GetNode<HealthBar>("HealthBar");
		healthBar.Initialize(MaxHealth);
		
		sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		
		player = GetTree().Root.FindChild("Player", true, false) as Player;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * Speed;
			MoveAndSlide();
			
			// Flip sprite based on movement direction
			if (sprite != null)
			{
				if (direction.X < 0)
					sprite.FlipH = true;
				else if (direction.X > 0)
					sprite.FlipH = false;
			}
		}
		
		// Handle animations
		UpdateAnimation();
	}

	private void UpdateAnimation()
	{
		if (sprite == null) return;
		
		// Play walk or idle based on velocity
		string targetAnimation = "idle";
		
		if (Velocity.Length() > 10.0f)
		{
			targetAnimation = "walk";
		}
		
		// Only change animation if different
		if (currentAnimation != targetAnimation)
		{
			sprite.Play(targetAnimation);
			currentAnimation = targetAnimation;
		}
	}

	public void SetEnemy(Enemy enemy)
	{
		parentEnemy = enemy;
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		currentHealth = Mathf.Max(0, currentHealth);
		healthBar.UpdateHealth(currentHealth);
		
		if (currentHealth <= 0)
		{
			if (parentEnemy != null)
				parentEnemy.OnAllyDied();
			QueueFree();
		}
	}
}

// Scripts/Player.cs
using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	[Export] public float MaxHealth = 100.0f;
	public float CurrentHealth { get; private set; }
	[Export] public float AttackDamage = 15.0f;
	
	// Projectile settings
	[Export] public PackedScene ProjectileScene;
	[Export] public float ProjectileSpeed = 400.0f;
	[Export] public Vector2 ThrowOffset = new Vector2(20, 0);
	
	private HealthBar healthBar;
	private float attackCooldown = 0.0f;
	[Export] public float AttackCooldownTime = 0.5f;
	
	private AnimatedSprite2D sprite;
	private Vector2 lastDirection = Vector2.Right;
	private bool isAttacking = false;
	private float attackAnimationTimer = 0.0f;
	[Export] public float AttackAnimationDuration = 0.3f;

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		healthBar = GetNode<HealthBar>("HealthBar");
		healthBar.Initialize(MaxHealth);
		
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		// Connect to animation finished signal if available
		if (sprite != null)
		{
			sprite.AnimationFinished += OnAnimationFinished;
		}
	}

	public override void _Process(double delta)
	{
		if (attackCooldown > 0) 
			attackCooldown -= (float)delta;
		
		if (attackAnimationTimer > 0)
		{
			attackAnimationTimer -= (float)delta;
			if (attackAnimationTimer <= 0)
			{
				isAttacking = false;
			}
		}
		
		// Handle attack
		if (Input.IsActionJustPressed("attack") && attackCooldown <= 0)
		{
			ThrowProjectile();
		}
		
		// Update animation based on state
		UpdateAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		// Update last direction for throwing
		if (direction.Length() > 0)
		{
			lastDirection = direction.Normalized();
			
			// Flip sprite based on direction
			if (direction.X < 0)
				sprite.FlipH = true;
			else if (direction.X > 0)
				sprite.FlipH = false;
		}
		
		Velocity = direction * Speed;
		MoveAndSlide();
	}

	private void UpdateAnimation()
	{
		if (sprite == null) return;
		
		// Don't interrupt attack animation
		if (isAttacking) return;
		
		// Play walk or idle based on velocity
		if (Velocity.Length() > 10.0f)
		{
			if (sprite.Animation != "walk")
				sprite.Play("walk");
		}
		else
		{
			if (sprite.Animation != "idle")
				sprite.Play("idle");
		}
	}

	private void OnAnimationFinished()
	{
		// When attack animation finishes, return to movement
		if (sprite.Animation == "attack")
		{
			isAttacking = false;
		}
	}

	private void ThrowProjectile()
	{
		if (ProjectileScene == null)
		{
			GD.PrintErr("ProjectileScene not set on Player!");
			return;
		}
		
		attackCooldown = AttackCooldownTime;
		
		// Play attack animation
		if (sprite != null)
		{
			sprite.Play("attack");
			isAttacking = true;
			attackAnimationTimer = AttackAnimationDuration;
		}
		
		// Spawn projectile
		var projectile = ProjectileScene.Instantiate<Projectile>();
		GetTree().Root.AddChild(projectile);
		
		// Calculate spawn position (in front of player)
		Vector2 spawnOffset = ThrowOffset;
		if (sprite != null && sprite.FlipH)
			spawnOffset.X = -spawnOffset.X;
			
		Vector2 spawnPosition = GlobalPosition + spawnOffset.Rotated(lastDirection.Angle());
		
		// Initialize projectile
		projectile.Initialize(spawnPosition, lastDirection);
		projectile.Speed = ProjectileSpeed;
		projectile.Damage = AttackDamage;
	}

	public void TakeDamage(float damage)
	{
		CurrentHealth -= damage;
		CurrentHealth = Mathf.Max(0, CurrentHealth);
		healthBar.UpdateHealth(CurrentHealth);
		
		if (CurrentHealth <= 0)
		{
			GD.Print("Player Died!");
			GetTree().ReloadCurrentScene();
		}
	}
}

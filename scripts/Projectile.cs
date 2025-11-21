// Scripts/Projectile.cs (Fixed for AnimatedSprite2D and TileMapLayer)
using Godot;

public partial class Projectile : Area2D
{
	[Export] public float Speed = 400.0f;
	[Export] public float Damage = 15.0f;
	[Export] public float Lifetime = 3.0f;
	[Export] public float RotationSpeed = 5.0f; // Spin while flying
	
	private Vector2 direction;
	private float lifetimeTimer = 0.0f;
	private bool hasHit = false;
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		
		// Get AnimatedSprite2D instead of Sprite2D
		sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		
		// Play the animation if it exists
		if (sprite != null && sprite.SpriteFrames != null)
		{
			// Play default animation or specific throw animation
			if (sprite.SpriteFrames.HasAnimation("throw"))
			{
				sprite.Play("throw");
			}
			else if (sprite.SpriteFrames.HasAnimation("default"))
			{
				sprite.Play("default");
			}
		}
	}

	public override void _Process(double delta)
	{
		// Move projectile
		Position += direction * Speed * (float)delta;
		
		// Spin the projectile (looks cool for thrown objects)
		if (sprite != null)
		{
			sprite.Rotate(RotationSpeed * (float)delta);
		}
		
		// Track lifetime
		lifetimeTimer += (float)delta;
		if (lifetimeTimer >= Lifetime)
		{
			DestroyProjectile();
		}
	}

	public void Initialize(Vector2 startPosition, Vector2 targetDirection)
	{
		GlobalPosition = startPosition;
		direction = targetDirection.Normalized();
		
		// Initial rotation to face direction
		Rotation = direction.Angle();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (hasHit) return;
		
		if (body is Enemy enemy)
		{
			enemy.TakeDamage(Damage);
			hasHit = true;
			DestroyProjectile();
		}
		else if (body is Ally ally)
		{
			ally.TakeDamage(Damage);
			hasHit = true;
			DestroyProjectile();
		}
		else if (body is StaticBody2D || body is TileMapLayer)
		{
			hasHit = true;
			DestroyProjectile();
		}
	}

	private void DestroyProjectile()
	{
		QueueFree();
	}
}

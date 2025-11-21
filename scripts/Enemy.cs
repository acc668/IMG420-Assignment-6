using Godot;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
	[Export] public float MaxHealth = 100.0f;
	public float CurrentHealth { get; private set; }
	
	public Player Player { get; private set; }
	[Export] public PackedScene AllyScene;
	
	[Export] public Godot.Collections.Array<Vector2> PatrolPoints = new();
	
	private float attackCooldown = 0.0f;
	[Export] public float AttackCooldownTime = 1.0f;
	private float summonCooldown = 0.0f;
	[Export] public float SummonCooldownTime = 5.0f;
	
	// Contact damage settings
	[Export] public float ContactDamage = 10.0f;
	[Export] public float ContactDamageCooldown = 1.0f;
	private float contactDamageCooldownTimer = 0.0f;
	
	public int CurrentAllyCount = 0;
	[Export] public int MaxAllies = 2;
	
	private Label stateLabel;
	private HealthBar healthBar;
	private Area2D detectionArea;
	private AnimatedSprite2D sprite;
	private string currentAnimation = "";

	private BTNode behaviorTreeRoot;

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		
		Player = GetTree().Root.FindChild("Player", true, false) as Player;
		
		stateLabel = GetNode<Label>("StateLabel");
		healthBar = GetNode<HealthBar>("HealthBar");
		healthBar.Initialize(MaxHealth);
		
		sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		
		behaviorTreeRoot = GetNode<BTNode>("BehaviorTree");
		
		// Setup contact damage using existing DetectionArea
		detectionArea = GetNodeOrNull<Area2D>("DetectionArea");
		if (detectionArea != null)
		{
			detectionArea.BodyEntered += OnBodyEntered;
		}
	}

	public override void _Process(double delta)
	{
		if (attackCooldown > 0) attackCooldown -= (float)delta;
		if (summonCooldown > 0) summonCooldown -= (float)delta;
		if (contactDamageCooldownTimer > 0) contactDamageCooldownTimer -= (float)delta;
		
		if (behaviorTreeRoot != null)
		{
			behaviorTreeRoot.Tick((float)delta);
		}
		
		// Handle animations based on velocity
		UpdateAnimation();
	}

	private void UpdateAnimation()
	{
		if (sprite == null) return;
		
		// Determine which animation to play based on velocity
		string targetAnimation = "idle";
		
		if (Velocity.Length() > 10.0f)
		{
			targetAnimation = "walk";
			
			// Flip sprite based on movement direction
			if (Velocity.X < 0)
				sprite.FlipH = true;
			else if (Velocity.X > 0)
				sprite.FlipH = false;
		}
		
		// Only change animation if it's different
		if (currentAnimation != targetAnimation)
		{
			sprite.Play(targetAnimation);
			currentAnimation = targetAnimation;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player player && contactDamageCooldownTimer <= 0)
		{
			player.TakeDamage(ContactDamage);
			contactDamageCooldownTimer = ContactDamageCooldown;
		}
	}

	public void TakeDamage(float damage)
	{
		CurrentHealth -= damage;
		CurrentHealth = Mathf.Max(0, CurrentHealth);
		healthBar.UpdateHealth(CurrentHealth);
		
		if (CurrentHealth <= 0)
		{
			QueueFree();
		}
	}

	public bool CanAttack()
	{
		return attackCooldown <= 0;
	}

	public void Attack(float damage)
	{
		if (Player != null && CanAttack())
		{
			// Play attack animation
			if (sprite != null)
			{
				sprite.Play("attack");
				currentAnimation = "attack";
			}
			
			Player.TakeDamage(damage);
			attackCooldown = AttackCooldownTime;
		}
	}

	public void SummonAlly(float spawnDistance)
	{
		if (summonCooldown > 0 || CurrentAllyCount >= MaxAllies || AllyScene == null)
			return;
		
		var ally = AllyScene.Instantiate<Ally>();
		GetParent().AddChild(ally);
		
		float angle = GD.Randf() * Mathf.Tau;
		Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance;
		ally.GlobalPosition = GlobalPosition + offset;
		
		ally.SetEnemy(this);
		CurrentAllyCount++;
		summonCooldown = SummonCooldownTime;
	}

	public void OnAllyDied()
	{
		CurrentAllyCount = Mathf.Max(0, CurrentAllyCount - 1);
	}

	public void SetState(string state)
	{
		if (stateLabel != null)
			stateLabel.Text = state;
	}
}

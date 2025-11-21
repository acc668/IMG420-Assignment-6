using Godot;

public partial class HealthBar : Node2D
{
    private float maxHealth;
    private ColorRect background;
    private ColorRect foreground;

    public override void _Ready()
    {
        background = new ColorRect();
        background.Size = new Vector2(40, 4);
        background.Position = new Vector2(-20, -30);
        background.Color = new Color(0.3f, 0.3f, 0.3f);
        AddChild(background);
        
        foreground = new ColorRect();
        foreground.Size = new Vector2(40, 4);
        foreground.Position = new Vector2(-20, -30);
        foreground.Color = new Color(0.0f, 1.0f, 0.0f);
        AddChild(foreground);
    }

    public void Initialize(float max)
    {
        maxHealth = max;
    }

    public void UpdateHealth(float current)
    {
        float ratio = current / maxHealth;
        foreground.Size = new Vector2(40 * ratio, 4);
        
        if (ratio > 0.5f)
            foreground.Color = new Color(0.0f, 1.0f, 0.0f);
        else if (ratio > 0.2f)
            foreground.Color = new Color(1.0f, 1.0f, 0.0f);
        else
            foreground.Color = new Color(1.0f, 0.0f, 0.0f);
    }
}
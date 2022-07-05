namespace Tests.Sample;

class TestHudGUI : GUILayer
{
    void _ButtonActivated()
    {
        if (Game.ActiveScene is TestScene scene)
        {
            var spriteRenderer = scene.PlayerEntity.GetComponent<SpriteRenderer>();
            
            var random = new Random();
            var r = random.Next(0, 256);
            var g = random.Next(0, 256);
            var b = random.Next(0, 256);

            spriteRenderer.Color = new Color(r, g, b);
        }
    }

    protected override void Load()
    {
        var button = new Button();
        button.Header = "RANDOMIZE CHARACTER COLOR";
        button.FontSize = 7;
        button.Foreground = Color.White;
        button.DefaultBackground = Color.Transparent;
        button.HoverBackground = new Color(Color.Black, 0.3f);
        button.PressingBackground = new Color(Color.Black, 0.15f);
        button.OnActivated += _ButtonActivated;

        var stackPanel = new StackPanel();
        stackPanel.FlowDirection = StackFlowDirection.TopToBottom;

        stackPanel.Sizes.Add(new(20));
        stackPanel.Add(button);

        Panel.Add(stackPanel);
        Panel.Margin = new(2);
        Panel.Width = 196;
        Panel.HorizontalAlign = HorizontalAlignment.Right;
    }
}
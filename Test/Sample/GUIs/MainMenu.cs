namespace Test.GameSample;

class MainMenu : GUILayer
{
    internal StackPanel _ButtonsStackPanel;
    internal Button _StartButton;
    internal Button _ExitButton;

    void TriggerStart()
    {
        Game.GUI.Hide(this);
        Game.EnterNextScene();
    }
    void TriggerExit()
    {
        Game.Exit();
    }  

    public override void Initialize()
    {
        _StartButton = new Button()
        {
            Header = "Start",
            DefaultBackground = Color.Transparent,
            HoverBackground = new Color(0x2F4040F0),
            PressingBackground = new Color(0x2F101090)
        };
        _ExitButton = new Button()
        {
            Header = "Exit",
            DefaultBackground = Color.Transparent,
            HoverBackground = new Color(0x2F4040F0),
            PressingBackground = new Color(0x2F101090)
        };

        _StartButton.OnActivated += TriggerStart;
        _ExitButton.OnActivated += TriggerExit;

        _ButtonsStackPanel = new StackPanel()
        {
            FlowDirection = StackFlowDirection.TopToBottom,
            Background = Color.Transparent,
            Sizes = new List<StackElementSize>()
            {
                new(1/2f),
                new(1/2f)
            },
            Content = new List<GUIElement>()
            {
                _StartButton,
                _ExitButton
            }
        };

        Panel.HorizontalAlign = HorizontalAlignment.Center;
        Panel.VerticalAlign = VerticalAlignment.Center;
        Panel.Width = 256;
        Panel.Height = 196;
        Panel.Add(_ButtonsStackPanel);
        Panel.Background = Color.BurlyWood;
    }

    public override void Dispose()
    {
        base.Dispose();

        _ButtonsStackPanel = null;
        _StartButton = null;
        _ExitButton = null;
    }

}
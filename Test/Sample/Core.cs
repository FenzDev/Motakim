namespace Test.GameSample;

class Core : GameManager
{
    public Core()
    {
        Input.RegisterKey("Left", new InputKey().AddKeyboardTrigger(Keys.Left).AddKeyboardTrigger(Keys.A));
        Input.RegisterKey("Up", new InputKey().AddKeyboardTrigger(Keys.Up).AddKeyboardTrigger(Keys.W));
        Input.RegisterKey("Right", new InputKey().AddKeyboardTrigger(Keys.Right).AddKeyboardTrigger(Keys.D));
        Input.RegisterKey("Down", new InputKey().AddKeyboardTrigger(Keys.Down).AddKeyboardTrigger(Keys.S));

        Game.Scenes = new List<Scene>()
        {
            new Scene0(),
            new Scene1()
        };
    }


    protected override void OnInitializing()
    {
        Game.GUI.Display(new MainMenu());
    }
}
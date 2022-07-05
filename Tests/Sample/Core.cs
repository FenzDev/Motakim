namespace Tests.Sample;

public class Core : GameManager
{
    public static IInputProvider Left;
    public static IInputProvider Up;
    public static IInputProvider Right;
    public static IInputProvider Down;

    public Core()
    {
        Game.Scenes.Add(new TestScene());
    
        Left = Input.RegisterKey("Left", new InputKey()).AddKeyboardTrigger(Keys.A).AddKeyboardTrigger(Keys.Left);
        Up = Input.RegisterKey("Up", new InputKey()).AddKeyboardTrigger(Keys.W).AddKeyboardTrigger(Keys.Up);
        Right = Input.RegisterKey("Right", new InputKey()).AddKeyboardTrigger(Keys.D).AddKeyboardTrigger(Keys.Right);
        Down = Input.RegisterKey("Down", new InputKey()).AddKeyboardTrigger(Keys.S).AddKeyboardTrigger(Keys.Down);
    }

    protected override void OnceInitialized()
    {
        Font.DefaultFont = Assets.Get<Font>("MainFont.json");
        Game.GUI.Display(new TestHudGUI());
    }
}
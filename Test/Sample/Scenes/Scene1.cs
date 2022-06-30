namespace Test.GameSample;

class Scene1 : Scene
{
    public Entity Player;

    protected override void Initialize()
    {
        Background = Color.CornflowerBlue;

        Player = CreateEntity("Player");
        Player.GetComponent<Transform>().Position = new Vector2(64f);
        Player.AddComponent<SpriteRenderer>();
        Player.AddComponent<PlayerMovement>();        
    }

    
}
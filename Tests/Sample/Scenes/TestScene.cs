namespace Tests.Sample;

class TestScene : Scene
{
    public Entity PlayerEntity;

    protected override void Load()
    {
        Background = Color.CornflowerBlue;
        
        PlayerEntity = CreateEntity("Player", new(32f, 32f));
        PlayerEntity.AddComponent(new SpriteRenderer(Assets.Get<Texture>("Player.png")));
        PlayerEntity.AddComponent<PlayerMovement>();
    } 
}
namespace Test.GameSample;

class PlayerMovement : Component, IUpdate
{
    static bool _IsSpriteLoaded;
    static Sprite _PlayerIdleSprite;
    static Sprite _PlayerMoveSprite;

    float Speed = 1.2f;
    PlayerState State;
    IInputProvider Left = Input.GetKey("Left");
    IInputProvider Up = Input.GetKey("Up");
    IInputProvider Right = Input.GetKey("Right");
    IInputProvider Down = Input.GetKey("Down");

    public PlayerMovement()
    {
        if (!_IsSpriteLoaded)
        {
            _PlayerIdleSprite = new Sprite( Assets.Get<Texture>("PlayerIdle.png"), new Rectangle(Point.Zero, new Point(24)), 60, new Point(12) );
            _PlayerMoveSprite = new Sprite( Assets.Get<Texture>("PlayerMove.png"), new Rectangle(Point.Zero, new Point(24)), 10, new Point(12) );
            _IsSpriteLoaded = true;
        }
    }
    public void Update()
    {
        var transform = Entity.GetComponent<Transform>();
        var spriteRenderer = Entity.GetComponent<SpriteRenderer>();
        
        var isMoving = false;
        if (Left.IsHolding)
        {
            isMoving = true;
            transform.Position.X -= Speed;
            transform.Scale.X = -
            1f;
        }
        else if (Right.IsHolding)
        {
            isMoving = true;
            transform.Position.X += Speed;
            transform.Scale.X = 1f;
        }
        if (isMoving) State = PlayerState.Moving;
        else State = PlayerState.Idle;

        switch (State)
        {
            case PlayerState.Idle:
                if (spriteRenderer.Sprite != _PlayerIdleSprite) spriteRenderer.Sprite = _PlayerIdleSprite;
                break;
            case PlayerState.Moving:
                if (spriteRenderer.Sprite != _PlayerMoveSprite) spriteRenderer.Sprite = _PlayerMoveSprite;
                break;
        }
    }
}

enum PlayerState
{
    Idle,
    Moving
}
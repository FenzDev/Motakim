namespace Tests.Sample;

class PlayerMovement : Component, IUpdate
{
    public static float Speed = 1.2f;

    public void Update()
    {
        var transform = Entity.GetComponent<Transform>();
        if (Core.Right.IsHolding)
        {
            transform.Translation.X += Speed;
        }
        else if (Core.Left.IsHolding)
        {
            transform.Translation.X -= Speed;
        }
        if (Core.Up.IsHolding)
        {
            transform.Translation.Y -= Speed;
        }
        else if (Core.Down.IsHolding)
        {
            transform.Translation.Y += Speed;
        }
    }
}
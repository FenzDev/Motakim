namespace Tests.Sample;

class PlayerMovement : Component, IUpdate
{
    public static float Speed = 1.2f;
    public static float DiagonalSpeed = Speed / MathF.Sqrt(2);

    public void Update()
    {
        var transform = Entity.GetComponent<Transform>();

        var movesHor = Core.Right.IsHolding || Core.Left.IsHolding;
        var movesVer = Core.Up.IsHolding || Core.Down.IsHolding;

        float xMovement, yMovement;
        xMovement = (Core.Right.Holding - Core.Left.Holding);
        yMovement = (Core.Down.Holding - Core.Up.Holding);

        if (movesHor && movesVer)
        {
            xMovement *= DiagonalSpeed;
            yMovement *= DiagonalSpeed;
        }
        else
        {
            xMovement *= Speed;
            yMovement *= Speed;
        }

        transform.Translation += new Vector2(xMovement, yMovement);
    }
}
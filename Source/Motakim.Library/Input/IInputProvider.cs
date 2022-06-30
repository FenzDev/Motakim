namespace Motakim
{
    public interface IInputProvider
    {
        float Holding { get; }
        bool IsHolding { get; }
        float Pressed { get; }
        bool IsPressed { get; }
        float Released { get; }
        bool IsReleased { get; }
    }
}
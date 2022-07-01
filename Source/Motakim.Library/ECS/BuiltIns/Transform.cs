using Microsoft.Xna.Framework;

namespace Motakim 
{
    public class Transform : Component 
    {
        public Transform() {}
        public Transform(float x, float y) : this(new Vector2(x, y)) {}
        public Transform(Vector2 position)
        {
            Position = position;
        }
        public Transform(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }
        public Transform(Vector2 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
        }
        public Transform(Vector2 position, Vector2 scale, float rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public Vector2 Position;
        public Vector2 Scale = new Vector2(1f);
        public float Rotation;
    }
}

using Microsoft.Xna.Framework;

namespace Motakim 
{
    public class Transform : Component 
    {
        public Transform() {}
        public Transform(float x, float y) : this(new Vector2(x, y)) {}
        public Transform(Vector2 translation)
        {
            Translation = translation;
        }
        public Transform(Vector2 translation, Vector2 scale)
        {
            Translation = translation;
            Scale = scale;
        }
        public Transform(Vector2 translation, float rotation)
        {
            Translation = translation;
            Rotation = rotation;
        }
        public Transform(Vector2 translation, Vector2 scale, float rotation)
        {
            Translation = translation;
            Scale = scale;
            Rotation = rotation;
        }

        public Vector2 Translation;
        public Vector2 Scale = new Vector2(1f);
        public float Rotation;
    }
}

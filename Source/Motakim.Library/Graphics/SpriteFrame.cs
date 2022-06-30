using Microsoft.Xna.Framework;

namespace Motakim
{
    public struct SpriteFrame
    {
        internal SpriteFrame(Texture texture, Rectangle texturePart, Vector2 origin)
        {
            Texture = texture;
            TexturePart = texturePart;
            Origin = origin;
        }

        internal Vector2 Origin;
        public Texture Texture;
        public Rectangle TexturePart;
    }
}
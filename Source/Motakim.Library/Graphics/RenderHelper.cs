using Microsoft.Xna.Framework;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace Motakim
{
    public sealed class RenderHelper
    {
        internal Microsoft.Xna.Framework.Graphics.SpriteBatch _SpriteBatch;
        internal float _ExtraScale = 1f;

        public int TextFontSize;
        public Font TextFont;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public string Text;
        public Texture Texture = null;
        public Rectangle? Part;
        public Color Color = Color.White;
        public float Rotation;
        public float LayerDepth;

        public void Reset()
        {
            TextFont = null;
            Position = Vector2.Zero;
            Origin = Vector2.Zero;
            Scale = Vector2.One;
            Text = string.Empty;
            Texture = null;
            Part = null;
            Color = Color.White;
            Rotation = 0f;
            LayerDepth = 0f;
        }

        public void DrawTexture()
        {
            XnaRectangle? rectangle = null;
            if (Part.HasValue) rectangle = Part.Value;

            var eff = SpriteEffects.None;
            var scale = Scale;
            if (Scale.X < 0)
            {
                scale.X *= -1f;
                eff = SpriteEffects.FlipHorizontally;
            }
            if (Scale.Y < 0)
            {
                scale.Y *= -1f;
                eff &= SpriteEffects.FlipVertically;
            }
            _SpriteBatch.Draw(Texture.GetTexture2D(Game._GameInstance.GraphicsDevice),
                Position * new Vector2(_ExtraScale),
                rectangle,
                Color,
                Rotation,
                Origin,
                scale * new Vector2(_ExtraScale),
                eff,
                LayerDepth);
            
        }

        public void DrawText()
        {
            var font = TextFont.GetFont((int)(TextFontSize * _ExtraScale));

            font.DrawText(
                _SpriteBatch,
                Text,
                Position * new Vector2(_ExtraScale),
                Color,
                Scale,
                Rotation,
                Origin * new Vector2(_ExtraScale),
                LayerDepth
            );
        }
        
    }
}
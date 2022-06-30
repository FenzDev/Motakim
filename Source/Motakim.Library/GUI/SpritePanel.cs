using Microsoft.Xna.Framework;

namespace Motakim
{
    public sealed class SpritePanel : GUIElement
    {
        public float SpriteRotation;
        public Vector2 SpriteScale = Vector2.One;
        public Sprite Sprite
        {
            get => SpriteInstance.Sprite;
            set => SpriteInstance = value.CreateInstance();  
        }
        public SpriteInstance SpriteInstance;
        public Color SpriteColor = Color.White;

        internal override void Render(RenderHelper render)
        {
            if (SpriteColor.A == 0) return;

            float drawX, drawY, drawXScale, drawYScale, drawOriginX, drawOriginY;

            if (_Bounds.IsEmpty) return;

            var width = Sprite.Width;
            var height = Sprite.Height;

            drawX = 0f;
            drawY = 0f;
            drawXScale = SpriteScale.X;
            drawYScale = SpriteScale.Y;

            switch (HorizontalAlign)
            {
                case HorizontalAlignment.Center:
                    drawX = _Bounds.X + _Bounds.Width / 2f;
                    drawOriginX = width / 2f;
                    break;
                case HorizontalAlignment.Left:
                    drawX = _Bounds.Left;
                    drawOriginX = 0f;
                    break;
                case HorizontalAlignment.Right:
                    drawX = _Bounds.Right;
                    drawOriginX = width;
                    break;
                default:
                    drawX = _Bounds.X;
                    drawXScale = _Bounds.X / width;
                    drawOriginX = 0f;
                    break;
            }
            switch (VerticalAlign)
            {
                case VerticalAlignment.Center:
                    drawY = _Bounds.Y + _Bounds.Height / 2f;
                    drawOriginY = height / 2f;
                    break;
                case VerticalAlignment.Top:
                    drawY = _Bounds.Top;
                    drawOriginY = 0f;
                    break;
                case VerticalAlignment.Bottom:
                    drawY = _Bounds.Bottom;
                    drawOriginY = height;
                    break;
                default:
                    drawY = _Bounds.Y;
                    drawYScale = _Bounds.Y / height;
                    drawOriginY = 0f;
                    break;
            }

            var frame = SpriteInstance.CurrentFrame;

            render.Texture = frame.Texture;
            render.Part = frame.TexturePart;
            render.Position = new Vector2(drawX, drawY);
            render.Origin = frame.Origin + new Vector2(drawOriginX, drawOriginY);
            render.Scale = new Vector2(drawXScale, drawYScale);
            render.Color = SpriteColor;

            render._SpriteBatch.Begin(samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
            render.DrawTexture();

            var scissorRect = new Rectangle
            (
                (int)(_Bounds.X * Game.GUI.Scale),
                (int)(_Bounds.Y * Game.GUI.Scale),
                (int)(_Bounds.Width * Game.GUI.Scale),
                (int)(_Bounds.Height * Game.GUI.Scale)
            );
            var oldScissorRect = Game._GameInstance.GraphicsDevice.ScissorRectangle;

            Game._GameInstance.GraphicsDevice.ScissorRectangle = scissorRect;
            render._SpriteBatch.End();
            Game._GameInstance.GraphicsDevice.ScissorRectangle = oldScissorRect;

            render.Reset();
            SpriteInstance.Update();
        }

    }
}
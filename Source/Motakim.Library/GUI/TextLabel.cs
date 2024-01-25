using Microsoft.Xna.Framework;

namespace Motakim
{
    public sealed class TextLabel : GUIElement
    {
        public string Text;
        public Font Font;
        public int FontSize = 9;
        public Color Foreground = Color.White;

        internal override void Render(RenderHelper render)
        {
            if (Font == null) return;

            if (Foreground.A == 0) return;

            float drawX, drawY, drawXScale, drawYScale, drawOriginX, drawOriginY;

            if (_Bounds.IsEmpty) return;

            var size = Font.MeasureText(FontSize, Text);
            var width = size.X;
            var height = size.Y;

            drawX = 0f;
            drawY = 0f;
            drawXScale = 1f;
            drawYScale = 1f;

            switch (HorizontalAlign)
            {
                case HorizontalAlignment.Center:
                    drawX = _Bounds.X + _Bounds.Width / 2f;
                    drawOriginX = 0.5f;
                    break;
                case HorizontalAlignment.Left:
                    drawX = _Bounds.Left;
                    drawOriginX = 0f;
                    break;
                case HorizontalAlignment.Right:
                    drawX = _Bounds.Right;
                    drawOriginX = 1f;
                    break;
                default:
                    drawX = _Bounds.X;
                    drawXScale = _Bounds.Width / width;
                    drawOriginX = 0f;
                    break;
            }
            switch (VerticalAlign)
            {
                case VerticalAlignment.Center:
                    drawY = _Bounds.Y + _Bounds.Height / 2f;
                    drawOriginY = 0.5f;
                    break;
                case VerticalAlignment.Top:
                    drawY = _Bounds.Top;
                    drawOriginY = 0f;
                    break;
                case VerticalAlignment.Bottom:
                    drawX = _Bounds.Bottom;
                    drawOriginY = 1f;
                    break;
                default:
                    drawY = _Bounds.Y;
                    drawYScale = _Bounds.Height / height;
                    drawOriginY = 0f;
                    break;
            }

            render.Text = Text;
            render.TextFont = Font;
            render.TextFontSize = FontSize;
            render.Position = new Vector2(drawX, drawY);
            render.Origin = new Vector2(drawOriginX, drawOriginY);
            render.Scale = new Vector2(drawXScale, drawYScale);
            render.Color = Foreground;

            render._SpriteBatch.Begin(samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
            render.DrawText();

            var graphicsDevice = Game._GameInstance.GraphicsDevice;

            var scissorRect = new Rectangle
            (
                (int)(_Bounds.X * Game.GUI.Scale),
                (int)(_Bounds.Y * Game.GUI.Scale),
                (int)(_Bounds.Width * Game.GUI.Scale),
                (int)(_Bounds.Height * Game.GUI.Scale)
            );
            var oldScissorRect = graphicsDevice.ScissorRectangle;

            graphicsDevice.ScissorRectangle = scissorRect;
            render._SpriteBatch.End();
            graphicsDevice.ScissorRectangle = oldScissorRect;

            render.Reset();
        }

    }
}
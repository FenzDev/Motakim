using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public class StackPanel : Panel
    {
        public StackFlowDirection FlowDirection;
        public List<StackElementSize> Sizes = new List<StackElementSize>();

        private void UpdateContentBounds()
        {
            var paddingBounds = _Bounds - Padding; 
            
            var currentPos = 0;
            for (var i = 0; i < Content.Count; i++)
            {
                var size = Sizes[i];
                
                int x, y, width, height;

                x = 0;
                y = 0;
                width = 0;
                height = 0;

                switch (FlowDirection)
                {
                    case StackFlowDirection.LeftToRight:
                        x = paddingBounds.Left + currentPos;
                        y = paddingBounds.Y;
                        width = (int)(paddingBounds.Width * size.Ratio) + size.Length;
                        height = paddingBounds.Height;
                        currentPos += width;
                        break;
                    case StackFlowDirection.RightToLeft:
                        x = paddingBounds.Right - currentPos;
                        y = paddingBounds.Y;
                        width = (int)(paddingBounds.Width * size.Ratio) + size.Length;
                        height = paddingBounds.Height;
                        currentPos += width;
                        break;
                    case StackFlowDirection.TopToBottom:
                        x = paddingBounds.X;
                        y = paddingBounds.Top + currentPos;
                        width = paddingBounds.Width;
                        height = (int)(paddingBounds.Height * size.Ratio) + size.Length;
                        currentPos += height;
                        break;
                    case StackFlowDirection.BottomToTop:
                        x = paddingBounds.X;
                        y = paddingBounds.Bottom - currentPos;
                        width = paddingBounds.Width;
                        height = (int)(paddingBounds.Height * size.Ratio) + size.Length;
                        currentPos += height;
                        break;
                }

                x = Math.Min(x, paddingBounds.Right);
                y = Math.Min(y, paddingBounds.Bottom);
                width = Math.Min(x + width, paddingBounds.Right) - x;
                height = Math.Min(y + height, paddingBounds.Bottom) - y;

                var elementBounds = new Rectangle(x, y, width, height);

                Content[i].Update(elementBounds);
            }
        }
        internal override void Update(Rectangle bounds)
        {
            UpdateBounds(bounds);

            UpdateContentBounds();
        }
        internal override void Render(RenderHelper render)
        {
            RenderPanelBackground(render);

            RenderContent(render);
        }
    }

    public struct StackElementSize
    {
        public StackElementSize(int length, float ratio)
        {
            Length = length;
            Ratio = ratio;
        }
        public StackElementSize(int length)
        {
            Length = length;
            Ratio = 0f;
        }
        public StackElementSize(float ratio)
        {
            Length = 0;
            Ratio = ratio;
        }
        
        // Length + Ratio
        public float Ratio;
        public int Length;
    }

    public enum StackFlowDirection
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }
}
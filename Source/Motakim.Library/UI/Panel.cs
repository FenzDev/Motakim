using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public class Panel : GUIElement
    {
        public int Width;
        public int Height;
        public Thickness Padding; 
        public Color Background;
        public List<GUIElement> ContentList = new List<GUIElement>();
        public IReadOnlyList<GUIElement> Content
        {
            get => ContentList.AsReadOnly();
            set
            {
                ContentList.Clear();
                AddRange(value);
            }
        }
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public GUIElement this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        internal void UpdateBounds(Rectangle bounds)
        {
            _Bounds = bounds - Margin;

            int rectX, rectY, rectWidth, rectHeight/*, drawOriginX, drawOriginY*/;

            if (_Bounds.IsEmpty) return;

            rectX = 0;
            rectY = 0;

            switch (HorizontalAlign)
            {
                case HorizontalAlignment.Center:
                    rectX = _Bounds.X + (_Bounds.Width - Width) / 2;
                    rectWidth = Width;
                    // drawOriginX = 0.5f;
                    break;
                case HorizontalAlignment.Left:
                    rectX = _Bounds.Left;
                    rectWidth = Width;
                    // drawOriginX = 0f;
                    break;
                case HorizontalAlignment.Right:
                    rectX = _Bounds.Right - Width;
                    rectWidth = Width;
                    // drawOriginX = 1f;
                    break;
                default:
                    rectX = _Bounds.X;
                    rectWidth = _Bounds.Width;
                    // drawOriginX = 0f;
                    break;
            }
            switch (VerticalAlign)
            {
                case VerticalAlignment.Center:
                    rectY = _Bounds.Y + (_Bounds.Height - Height) / 2;
                    rectHeight = Height;
                    break;
                case VerticalAlignment.Top:
                    rectY = _Bounds.Top;
                    rectHeight = Height;
                    break;
                case VerticalAlignment.Bottom:
                    rectY = _Bounds.Bottom - Height;
                    rectHeight = Height;
                    break;
                default:
                    rectY = _Bounds.Y;
                    rectHeight = _Bounds.Height;
                    break;
            }

            _Bounds = new Rectangle(rectX, rectY, rectWidth, rectHeight);
        }
        internal void RenderPanelBackground(RenderHelper render)
        { 
            if (Background.A == 0) return;

            render.Texture = Texture.Pixel;
            render.Position = new Vector2(_Bounds.X, _Bounds.Y);
            //render.Origin = new Vector2(drawOriginX, drawOriginY);
            render.Scale = new Vector2(_Bounds.Width, _Bounds.Height);
            render.Color = Background;

            render._SpriteBatch.Begin();
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
        }
        internal void RenderContent(RenderHelper render)
        {
            foreach (var element in Content)
            {
                element.Render(render);
            }
        }

        internal override void Update(Rectangle bounds)
        {
            UpdateBounds(bounds);

            foreach (var element in Content)
            {
                element.Update(_Bounds);
            }
        }
        internal override void Render(RenderHelper render)
        {
            RenderPanelBackground(render);
            
            RenderContent(render);
        }
        
        public void Add(GUIElement element)
        {
            element.Container = this;
            ContentList.Add(element);
        }       
        public void AddRange(IEnumerable<GUIElement> elements)
        {
            foreach (var element in elements)
            {
                element.Container = this;
            }
            ContentList.AddRange(elements);
        }       
        public void Insert(int index, GUIElement element)
        {
            element.Container = this;
            ContentList.Insert(index, element);
        }       
        public void InsertRange(int index, IEnumerable<GUIElement> elements)
        {
            foreach (var element in elements)
            {
                element.Container = this;
            }

            ContentList.InsertRange(index, elements);
        }       
        public void Remove(GUIElement element)
        {
            element.Container = null;
            var index = ContentList.IndexOf(element);
            RemoveAt(index);
        }
        public void RemoveRange(int index, int count)
        {
            for (var i = index; i < index + count; i++)
            {
                ContentList[i].Container = null;
            }
            ContentList.RemoveRange(index, count);
        }
        public void RemoveAt(int index)
        {
            ContentList[index].Container = null;
            ContentList.RemoveAt(index);
        }
        public void Clear()
        {
            foreach (var element in ContentList)
            {
                element.Container = null;
            }
            ContentList.Clear();
        }
    }
    
}
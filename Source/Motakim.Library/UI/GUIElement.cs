using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public abstract class GUIElement
    {
        internal Rectangle _Bounds;
        public string[] Tags;
        public HashSet<string> TagHashSet
        {
            get => Tags.ToHashSet();
            set => Tags = value.ToArray();
        }
        public bool IsEnabled;
        public bool IsVisible;
        public Thickness Margin;
        public HorizontalAlignment HorizontalAlign;
        public VerticalAlignment VerticalAlign;

        public Panel Container;

        internal virtual void Update(Rectangle bounds)
        {
            _Bounds = bounds - Margin;
        }
        internal abstract void Render(RenderHelper render);
    }
}
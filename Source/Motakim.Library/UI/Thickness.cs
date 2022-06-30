using Microsoft.Xna.Framework;

namespace Motakim
{
    public struct Thickness
    {
        public Thickness(int value)
        {
            Left = value;
            Top = value;
            Right = value;
            Bottom = value;
        }
        public Thickness(int h, int v)
        {
            Left = h;
            Top = v;
            Right = h;
            Bottom = v;
        }
        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public override bool Equals(object obj)
        {
            return (Thickness)obj == this;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Left.GetHashCode();
                hash = hash * 23 + Top.GetHashCode();
                hash = hash * 23 + Right.GetHashCode();
                hash = hash * 23 + Bottom.GetHashCode();
                return hash;
            }
        }
        public override string ToString()
        {
            return $"{Left}, {Top}, {Right}, {Bottom}";
        }

        public static bool operator ==(Thickness a, Thickness b)
        {
            return (
                (a.Left == b.Left) &&
                (a.Right == b.Right) && 
                (a.Top == b.Top) && 
                (a.Bottom == b.Bottom) ); 
        }
        public static bool operator !=(Thickness a, Thickness b)
        {
            return !(a == b); 
        }
        public static Rectangle operator +(Rectangle a, Thickness b)
        {
            var x = (a.X - b.Left);
            var y = (a.Y - b.Top);
            var width = (a.Width + b.Right + b.Left);
            var height = (a.Height + b.Bottom + b.Top);

            if (width < 0) width = 0;
            if (height < 0) height = 0; 

            return new Rectangle(x, y, width, height);
        }
        public static Rectangle operator -(Rectangle a, Thickness b)
        {
            var x = (a.X + b.Left);
            var y = (a.Y + b.Top);
            var width = (a.Width - b.Right - b.Left);
            var height = (a.Height - b.Bottom - b.Top);

            if (width < 0) width = 0;
            if (height < 0) height = 0; 

            return new Rectangle(x, y, width, height);
        }
    }
}
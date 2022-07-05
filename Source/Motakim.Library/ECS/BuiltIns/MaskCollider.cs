using Microsoft.Xna.Framework;

namespace Motakim
{
    public class MaskCollider : Component
    {
        public Rectangle Mask;

        public MaskCollider() {}
        public MaskCollider(int left, int top, int right, int bottom) : this(new Rectangle(left, top, right - left, bottom - top)) {}
        public MaskCollider(Rectangle mask)
        {
            Mask = mask;
        }

        public Rectangle GetTransformedMask()
        {
            if (Entity.HasComponent<Transform>(out var transform))
            {
                var location = (transform.Translation + new Vector2(Mask.X, Mask.Y) * transform.Scale).ToPoint();
                var size = (new Vector2(Mask.Width, Mask.Height) * transform.Scale).ToPoint();

                return new Rectangle(location, size);
            }
            return Mask;
        }
        public bool IsColidingWith(Entity entity)
        {
            if (entity.HasComponent<MaskCollider>(out var otherCollisionMask))
            {
                if (entity.HasComponent<Transform>(out var otherTransform) && Entity.HasComponent<Transform>(out var transform))
                {
                    return IsColidingWith(otherCollisionMask.GetTransformedMask());
                }
            }
            return false;
        }
        public bool IsColidingWith(Rectangle rectangle)
        { 
            return GetTransformedMask().Intersects(rectangle);
        }
        public bool IsColidingWith(Vector2 point)
        { 
            return GetTransformedMask().Contains(point);
        }
        public bool IsColidingWith(Point point)
        { 
            return GetTransformedMask().Contains(point);
        }
    }
}
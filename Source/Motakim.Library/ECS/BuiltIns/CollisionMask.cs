using Microsoft.Xna.Framework;

namespace Motakim
{
    public class CollisionMask : Component
    {
        public Rectangle Mask;

        public CollisionMask() {}
        public CollisionMask(Rectangle mask) : this(mask, false) {}
        public CollisionMask(Rectangle mask, bool IsSolid)
        {
            Mask = mask;
            IsSolid = true;
        }

        public Rectangle GetTransformedMask()
        {
            if (Entity.HasComponent<Transform>(out var transform))
            {
                var location = (transform.Position + new Vector2(Mask.X, Mask.Y) * transform.Scale).ToPoint();
                var size = (new Vector2(Mask.Width, Mask.Height) * transform.Scale).ToPoint();

                return new Rectangle(location, size);
            }
            return Mask;
        }
        public bool IsColidingWith(Entity entity)
        {
            if (entity.HasComponent<CollisionMask>(out var otherCollisionMask))
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
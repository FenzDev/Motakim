using Microsoft.Xna.Framework;

namespace Motakim {
    public class SpriteRenderer : Component, IRender
    {
        public Color Color = Color.White;
        public float Depth;
        public Sprite Sprite
        {
            get => (SpriteInstance == null)? null: SpriteInstance.Sprite;
            set => SpriteInstance = value.CreateInstance();
        }
        public SpriteInstance SpriteInstance;

        public SpriteRenderer() {}
        public SpriteRenderer(Texture texture) : this(new Sprite(texture)) {}
        public SpriteRenderer(Texture texture, Color color, float depth) : this(new Sprite(texture), color, depth) {}
        public SpriteRenderer(Sprite sprite) : this(sprite.CreateInstance(), Color.White) {}
        public SpriteRenderer(Sprite sprite, Color color, float depth = 0f) : this(sprite.CreateInstance(), color, depth) {}
        public SpriteRenderer(SpriteInstance spriteInstance, Color color, float depth = 0f)
        {
            SpriteInstance = spriteInstance;
            Color = color;
            Depth = depth;
        }

        public void Render(RenderHelper render)
        {
            if (Sprite == null) return;

            if (Entity.HasComponent<Transform>(out var transform))
            {
                var frame = SpriteInstance.CurrentFrame;
                
                render.Position = transform.Translation;
                render.Scale = transform.Scale;
                render.Rotation = transform.Rotation;
                render.Origin = frame.Origin;
                render.Part = frame.TexturePart;
                render.Color = Color;
                render.Texture = frame.Texture;
                render.LayerDepth = Depth;

                render.DrawTexture();

                SpriteInstance.Update();
            }
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public sealed class Sprite
    {
        private bool _IsSpriteSheet;
        private Texture[] _Textures;
        public Rectangle? FrameRectangle;
        public Point Offest;
        public int FrameDuration = 60;
        public SpriteFrame[] Frames => GetFrames().ToArray();
        public int Width => (_IsSpriteSheet)? FrameRectangle.Value.Width: _Textures[0].Width; 
        public int Height => (_IsSpriteSheet)? FrameRectangle.Value.Height: _Textures[0].Height; 

        public IEnumerable<SpriteFrame> GetFrames()
        {
            if (_IsSpriteSheet)
            {
                var frameRectangle = FrameRectangle.Value;
                var x = frameRectangle.X;
                var y = frameRectangle.Y;
                var width = frameRectangle.Width;
                var height = frameRectangle.Height;

                var texture = _Textures[0];

                var framesCount = (texture.Width - frameRectangle.X ) / frameRectangle.Width;

                for (var i = 0; i < framesCount; i++)
                {
                    var rect = new Rectangle(x + width * i, y, width, height);
                    yield return new SpriteFrame(texture, rect, Offest.ToVector2());
                }
            }
            else
            {
                foreach (var texture in _Textures)
                {
                    var rect = new Rectangle(0, 0, texture.Width, texture.Height);
                    yield return new SpriteFrame(texture, rect, Offest.ToVector2());    
                }
            }
        }
        public Sprite(Texture frame) : this(frame, Point.Zero) {}
        public Sprite(Texture frame, Point offest)
        {
            Offest = offest;
            _Textures = new Texture[] { frame };
        }
        public Sprite(Texture spriteSheet, Rectangle frameRectangle, int frameDuration) : this(spriteSheet, frameRectangle, frameDuration, Point.Zero) {}
        public Sprite(Texture spriteSheet, Rectangle frameRectangle, int frameDuration, Point offest)
        {
            _IsSpriteSheet = true;
            FrameRectangle = frameRectangle;
            _Textures = new Texture[] { spriteSheet };
            FrameDuration = frameDuration;
            Offest = offest;
        }
        public Sprite(Texture[] frames, int frameDuration) : this(frames, frameDuration, Point.Zero) {}
        public Sprite(Texture[] frames, int frameDuration, Point offest)
        {
            _Textures = frames;
            FrameDuration = frameDuration;
            Offest = offest;
        }

        public SpriteInstance CreateInstance() => new SpriteInstance(this, 0, true);
    }
}
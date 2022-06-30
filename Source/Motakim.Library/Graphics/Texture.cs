using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Motakim
{
    public sealed class Texture : IAsset
    {
        internal static Texture _Pixel;
        public static Texture Pixel => _Pixel;
        
        internal Texture2D Texture2D; 
        private bool _IsDisposed;
        public string Name { get; private set; } = "Texture";
        public int Width { get; private set; }
        public int Height { get; private set; }
        public uint[] TextureMap { get; internal set; }
        public bool IsDisposed => _IsDisposed;

        public Texture() : this(0, 0) {}
        public Texture(int width, int height)
        {
            Width = width;
            Height = height;
            TextureMap = new uint[Width * Height];
        }

        public void Load(Stream stream)
        {
            if (Texture2D != null) Texture2D.Dispose();

            Texture2D = Texture2D.FromStream(Game._GameInstance.GraphicsDevice, stream);
            
            Name = Texture2D.Name;

            Width = Texture2D.Width;
            Height = Texture2D.Height;
            TextureMap = new uint[Width * Height];

            Texture2D.GetData(TextureMap);
        }

        internal Texture2D GetTexture2D(GraphicsDevice graphicsDevice)
        {
            if (Texture2D == null)
            {
                Texture2D = new Texture2D(graphicsDevice, Width, Height);
            }

            return Texture2D;
        }

        public void Flush()
        {
            if (Texture2D == null)
            {
                Texture2D = new Texture2D(Game._GameInstance.GraphicsDevice, Width, Height);
            }
            
            Texture2D.SetData(TextureMap);   
        }

        public void Dispose()
        {
            if (Texture2D.Equals(null)) Texture2D.Dispose();
            _IsDisposed = true;
        }

        public override string ToString() => Name;
    }
}
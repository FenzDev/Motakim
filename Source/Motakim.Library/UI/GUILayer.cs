using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;

namespace Motakim
{
    public abstract class GUILayer : Utilities.DisposableObject
    {
        public Panel Panel;

        public abstract void Initialize();

        internal void Update()
        {
            var game = Game._GameInstance;
            var rect = new Rectangle(0, 0, (int)(Game.DisplayWidth / Game.GUI.Scale), (int)(Game.DisplayHeight / Game.GUI.Scale));
            
            Panel.Update(rect);
        }
        internal void Render()
        {
            var game = Game._GameInstance;
            
            Panel.Render(new RenderHelper() { _ExtraScale = Game.GUI.Scale, _SpriteBatch = game.SpriteBatch });
        }

        internal void Load()
        {
            Panel = new Panel();
            Initialize();
        }
        internal void Unload()
        {
            Panel = null;
            Dispose();
        }

    }
}
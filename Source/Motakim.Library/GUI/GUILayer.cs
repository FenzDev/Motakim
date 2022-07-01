using System.IO;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public abstract class GUILayer
    {
        public Panel Panel;

        public virtual void Load() {}
        public virtual void Unload() {}

        ~GUILayer()
        {
            Dispose();
        }
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

        internal void Initialize()
        {
            Panel = new Panel();
            Load();
        }
        internal void Dispose()
        {
            Panel = null;
            Unload();
        }

    }
}
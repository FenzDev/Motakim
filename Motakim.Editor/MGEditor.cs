using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Motakim.Editor
{
    class MGEditor : Game
    {
        public MGEditor()
        {
            GraphicsService = new GraphicsDeviceManager(this);
        }

        GraphicsDeviceManager GraphicsService;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            base.Draw(gameTime);
        }
    }
}
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Motakim.Engine
{
    public class GameCore : Game
    {
        public GameCore()
        {
            GraphicsService = new GraphicsDeviceManager(this);
        }

        public GraphicsDeviceManager GraphicsService;
    }
}

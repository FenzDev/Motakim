using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using XnaGame = Microsoft.Xna.Framework.Game;
using Microsoft.Xna.Framework.Graphics;

namespace Motakim {
    sealed class MGGame : XnaGame {
        
        public MGGame()
        {
            Graphics = new GraphicsDeviceManager(this);
        }

        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        protected override void LoadContent()
        {
            if (Game.Manager != null) Game.Manager.OnInitializing();

            IsMouseVisible = Game.IsMouseVisible;
            Graphics.PreferredBackBufferWidth = Game.DisplayWidth;
            Graphics.PreferredBackBufferHeight = Game.DisplayHeight;
            Graphics.IsFullScreen = Game.IsFullScreen;
            Graphics.ApplyChanges();

            Game.IsGameRunning = true;

            foreach (var scene in Game.Scenes)
            {
                if (scene.LoadWithGame)
                {
                    scene.Initialize();
                }

            }

            if (Game.ActiveScene != null)
            {
                Game.StartScene(false);
            }

            Input.Initialize(this);

            Texture._Pixel = new Texture(1, 1);
            Texture._Pixel.TextureMap[0] = uint.MaxValue;
            Texture._Pixel.Flush();

            Game.ActiveScene.Initialize();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();

            if (Game.Manager != null) Game.Manager.OnceInitialized();
        }

        protected override void UnloadContent()
        {
            if (Game.Manager != null) Game.Manager.OnFinalizing();
            
            base.UnloadContent();
            
            foreach (var scene in Game.Scenes)
            {
                if (!scene.IsLoaded) continue; 
                scene.Dispose();
            }

            Assets.UnloadAll();

            Game.IsGameRunning = false;

            if (Game.Manager != null) Game.Manager.OnceFinalized();
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Game.Manager != null) Game.Manager.OnRendering();

            if (Game.ActiveScene != null)
                GraphicsDevice.Clear(Game.ActiveScene.Background);
            else
                GraphicsDevice.Clear(Color.Black);

            var scene = Game.ActiveScene;

            if (scene != null)
            {
                foreach (var group in scene.RenderComponentGroups)
                {
                    if (!group.Entity.Enabled) continue;
                    foreach (var component in group)
                    {
                        if (!component.Enabled) continue;

                        SpriteBatch.Begin(transformMatrix: scene.GetCameraMatrix(), samplerState: SamplerState.PointClamp);
                        
                        var renderHelper = new RenderHelper() { _ExtraScale = Game.ScenePixelScaling, _SpriteBatch = SpriteBatch };
                        ((IRender)component).Render(renderHelper);

                        SpriteBatch.End();
                    }
                }
            }

            Game.GUI.Render();

            base.Draw(gameTime);

            if (Game.Manager != null) Game.Manager.OnceRendered();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Game.Manager != null) Game.Manager.OnUpdating();
            
            Input.Update();
            Game.GUI.Update();

            if (Input._KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Game.Exit();
            }

            var scene = Game.ActiveScene;
            
            if (scene != null)
            {
                foreach (var group in scene.UpdateComponentGroups)
                {
                    if (!group.Entity.Enabled) continue;
                    foreach (var component in group)
                    {
                        if (!component.Enabled) continue;

                        ((IUpdate)component).Update();
                    }
                }
            }

            if (scene.CameraTarget != null)
            {
                if (scene.CameraTarget.Scene != null && scene.CameraTarget.HasComponent<Transform>(out var transform))
                {
                    scene.Camera = transform.Translation;
                } 
            }

            if (!scene.IsCameraFree)
            {
                var l = scene.CameraBounds.Left + (int)(Game.DisplayWidth / Game.ScenePixelScaling) / 2;
                var t = scene.CameraBounds.Top + (int)(Game.DisplayHeight / Game.ScenePixelScaling) / 2;
                var r = scene.CameraBounds.Right - (int)(Game.DisplayWidth / Game.ScenePixelScaling) / 2;
                var b = scene.CameraBounds.Bottom - (int)(Game.DisplayHeight / Game.ScenePixelScaling) / 2;

                scene.Camera.X = Math.Max(scene.Camera.X, l);
                scene.Camera.X = Math.Min(scene.Camera.X, r);
                scene.Camera.Y = Math.Max(scene.Camera.Y, t);
                scene.Camera.Y = Math.Min(scene.Camera.Y, b);
            }

            if (Game._ChangeSceneNextUpdate)
            {
                Game.StartScene(true);
            }

            base.Update(gameTime);

            if (Game.Manager != null) Game.Manager.OnceUpdated();
        }
    }
}

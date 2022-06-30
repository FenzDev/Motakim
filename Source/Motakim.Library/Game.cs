using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public static class Game 
    {
        private static SoundEffectInstance _MusicSFXInstance;
        private static bool _IsFullScreen = false;
        private static bool _IsMouseVisible = true;
        private static int _DisplayWidth = 1024;
        private static int _DisplayHeight = 768;
        private static bool _AltF4QuitsGame = true;
        internal static MGGame _GameInstance;
        internal static bool _ChangeSceneNextUpdate;
        internal static int _ChangeSceneIndex;
        internal static GameManager _Manager;

        /// <summary>
        /// Optional game service, recives special events of the game for an easy management.
        /// </summary>        
        public static GameManager Manager
        {
            get => _Manager;
            set => _Manager = value;
        }
        public static float ScenePixelScaling = 4f;
        public static bool LoopScenes;
        public static bool IsMouseVisible
        {
            get => _IsMouseVisible;
            set
            {
                _IsMouseVisible = value;

                if (_GameInstance != null) 
                    _GameInstance.IsMouseVisible = value;
            }
        }
        public static bool IsFullScreen
        {
            get => _IsFullScreen;
            set
            {
                _IsFullScreen = value;

                if (_GameInstance != null) 
                    _GameInstance.Graphics.IsFullScreen = value;
            }
        }
        public static int DisplayWidth
        {
            get => _DisplayWidth;
            set
            {
                _DisplayWidth = value;

                if (_GameInstance != null) 
                    _GameInstance.Graphics.PreferredBackBufferWidth = value;
            }
        }
        public static int DisplayHeight
        {
            get => _DisplayHeight;
            set
            {
                _DisplayHeight = value;

                if (_GameInstance != null) 
                    _GameInstance.Graphics.PreferredBackBufferHeight = value;
            }
        }
        public static bool AltF4QuitsGame 
        {
            get => _AltF4QuitsGame;
            set
            {
                _AltF4QuitsGame = value;

                if (_GameInstance != null) 
                    _GameInstance.Window.AllowAltF4 = value;
            }
        }
        public static bool EscapeQuitsGame { get; set; }
        public static bool IsGameRunning { get; internal set; }
        public static Scene ActiveScene { get; private set; }
        public static int CurrentSceneIndex { get; private set; }
        public static List<Scene> Scenes { get; set; } = new List<Scene>();
        public static GUIManager GUI = new GUIManager();
        public static float MusicVolume
        {
            get => _MusicSFXInstance.Volume;
            set => _MusicSFXInstance.Volume = value;
        }
        public static SoundEffect Music
        {
            get => _MusicSFXInstance.SoundEffect;
            set => _MusicSFXInstance = SoundEffect.Play(value, MusicVolume, 1.0f, true, 0f);
        }

        internal static void StartScene(bool change)
        {
            if (change)
            {
                if (Game.Manager != null) Game.Manager.OnSceneEnding();
    
                ActiveScene.Leaving();
                if (!ActiveScene.LoadWithGame)
                {
                    ActiveScene.Unload();
                }

                if (Game.Manager != null) Game.Manager.OnceSceneEnded();
        
                ActiveScene = Scenes[_ChangeSceneIndex];
            }

            if (Game.Manager != null) Game.Manager.OnSceneStarting();
            
            ActiveScene.Entering();
            if (!ActiveScene.LoadWithGame)
            {
                ActiveScene.Load();
            }

            if (Game.Manager != null) Game.Manager.OnceSceneStarted();

            _ChangeSceneNextUpdate = false;
        }
        static void WrapScene()
        {
            if (CurrentSceneIndex < 0) CurrentSceneIndex += Scenes.Count; 
            if (CurrentSceneIndex > 0) CurrentSceneIndex -= Scenes.Count; 
        }

        public static void Run() => Run(null);
        public static void Run(GameManager manager) 
        {
            if (Game.Manager != null) Game.Manager.OnSceneStarting();
            
            Manager = manager;
            ActiveScene = Scenes[0];

            if (Game.Manager != null) Game.Manager.OnceSceneStarted();

            _GameInstance = new MGGame();
            _GameInstance.Run();

            _GameInstance.Dispose();

        }
        public static void EnterPreviousScene()
        {
            _ChangeSceneNextUpdate = true;
            _ChangeSceneIndex = CurrentSceneIndex - 1;
            if (LoopScenes) WrapScene();
            else if (_ChangeSceneIndex > Scenes.Count) Exit();
        }
        public static void EnterNextScene()
        {
            _ChangeSceneNextUpdate = true;
            _ChangeSceneIndex = CurrentSceneIndex + 1;
            if (LoopScenes) WrapScene();
            else if (_ChangeSceneIndex > Scenes.Count) Exit();
        }
        public static void EnterScene(int index)
        {
            _ChangeSceneNextUpdate = true;
            _ChangeSceneIndex = index;
            if (LoopScenes) WrapScene();
            else if (_ChangeSceneIndex > Scenes.Count) Exit(); 
        }
        public static void Exit()
        {
            if (IsGameRunning)
            {
                _GameInstance.Exit();
            }
        }
        public static void ApplyDisplayChanges()
        {
            _GameInstance.Graphics.ApplyChanges();
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Motakim.Utilities;

namespace Motakim
{
    public static class Input
    {
        private static int _PreviousScrollWheelValue;
        internal static int _ScrollWheelValue;
        private static Dictionary<string, InputKey> _Keys = new Dictionary<string, InputKey>();
        internal static KeyboardState _KeyboardState;
        internal static MouseState _MouseState;
        public static InputKey UIEnter = new InputKey().AddKeyboardTrigger(Keys.Enter);
        public static InputKey UILeft = new InputKey().AddKeyboardTrigger(Keys.Left);
        public static InputKey UIUp = new InputKey().AddKeyboardTrigger(Keys.Up);
        public static InputKey UIRight = new InputKey().AddKeyboardTrigger(Keys.Right);
        public static InputKey UIDown = new InputKey().AddKeyboardTrigger(Keys.Down);
        public static IReadOnlyDictionary<string, InputKey> InputKeys => _Keys.AsReadOnly();

        internal static void Initialize(MGGame game)
        {

        }
        internal static void Update()
        {
            _KeyboardState = Keyboard.GetState();
            _MouseState = Mouse.GetState();

            _ScrollWheelValue = _MouseState.ScrollWheelValue - _PreviousScrollWheelValue;
            _PreviousScrollWheelValue = _MouseState.ScrollWheelValue;

            foreach (var key in _Keys)
            {
                key.Value.Update();
            }
            UIEnter.Update();
            UILeft.Update();
            UIUp.Update();
            UIRight.Update();
            UIDown.Update();
        }
        public static InputKey RegisterKey(string name, InputKey key)
        {
            _Keys.Add(name, key);
            return key;
        }
        public static void UnregisterKey(string name) => _Keys.Remove(name);
        public static InputKey GetKey(string name) => _Keys[name];
        public static Point MouseGetPosition() => _MouseState.Position;
        public static IInputProvider KeyboardKeyState(Keys key)
        {
            var input = new InputKey().AddKeyboardTrigger(key);
            input.Update();
            return input;
        }
        public static IInputProvider MouseButtonsState(MouseButtons button)
        {
            var input = new InputKey().AddMouseButtonTrigger(button);
            input.Update();
            return input;
        }

    }
}
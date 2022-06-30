using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Motakim
{
    public sealed class InputKey : IInputProvider
    {
        private bool _HasBeenPressed;
        private bool _HasBeenReleased;
        private List<_Trigger> Triggers = new List<_Trigger>();

        public bool IsHolding { get; private set; }
        public float Holding => IsHolding? 1f: 0f;
        public bool IsPressed { get; private set; }
        public float Pressed => IsPressed? 1f: 0f;
        public bool IsReleased { get; private set; }
        public float Released => IsReleased? 1f: 0f;

        internal void Update()
        {
            bool haveBreak = false;
            foreach (var trigger in Triggers)
            {
                switch (trigger.Type)
                {
                    case _TriggerType.Keyboard:
                        var key = (Keys)trigger.Keyboard;
                        
                        if (Input._KeyboardState.IsKeyDown(key))
                        {
                            haveBreak = true;
                            TriggerDown();
                        }
                        else
                        {
                            TriggerUp();
                        }
                        break;
                    case _TriggerType.MouseButton:
                        
                        var mState = false;
                        switch (trigger.Mouse)
                        {
                            case MouseButtons.Left:
                                mState = Input._MouseState.LeftButton == ButtonState.Pressed;
                                break;
                            case MouseButtons.Right:
                                mState = Input._MouseState.RightButton == ButtonState.Pressed;
                                break;
                            case MouseButtons.Middle:
                                mState = Input._MouseState.MiddleButton == ButtonState.Pressed;
                                break;
                        }      

                        if (mState)
                        {
                            haveBreak = true;
                            TriggerDown();
                        }
                        else
                        {
                            TriggerUp();
                        }
                        break;
                    case _TriggerType.MouseScrollWheel:
                        var mswState = false;
                        switch (trigger.MouseWheel)
                        {
                            case MouseWheel.Up:
                                mswState = Input._ScrollWheelValue < 0;
                                break;
                            case MouseWheel.Down:
                                mswState = Input._ScrollWheelValue > 0;
                                break;
                        }      

                        if (mswState)
                        {
                            haveBreak = true;
                            TriggerDown();
                        }
                        else
                        {
                            TriggerUp();
                        }
                        break;
                }
                if (haveBreak) break;
            }
        }
        internal void TriggerDown()
        {
            IsHolding = true;
            if (_HasBeenPressed)
            {
                IsPressed = false;
                _HasBeenPressed = false;
            }
            else
            {
                IsPressed = true;
                _HasBeenPressed = true;
            }
        }
        internal void TriggerUp()
        {
            IsHolding = false;
            if (_HasBeenReleased)
            {
                IsReleased = false;
                _HasBeenReleased = false;
            }
            else
            {
                IsReleased = true;
                _HasBeenReleased = true;
            }
        }

        public InputKey AddKeyboardTrigger(Keys key)
        {
            Triggers.Add(new _Trigger() { Type = _TriggerType.Keyboard, Keyboard = key });
            return this;
        }

        public InputKey AddMouseButtonTrigger(MouseButtons button)
        {
            Triggers.Add(new _Trigger() { Type = _TriggerType.MouseButton, Mouse = button });
            return this;
        }

        public InputKey AddMouseScrollTrigger(MouseWheel scroll)
        {
            Triggers.Add(new _Trigger() { Type = _TriggerType.MouseScrollWheel, MouseWheel = scroll });
            return this;
        }

        struct _Trigger
        {
            public _TriggerType Type;
            public Keys Keyboard;
            public MouseButtons Mouse;
            public MouseWheel MouseWheel;

        }
        enum _TriggerType
        {
            None,
            Keyboard,
            MouseButton,
            MouseScrollWheel,
        }
    }
}
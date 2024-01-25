using System;
using Microsoft.Xna.Framework;

namespace Motakim
{
    public sealed class Button : Panel
    {
        TextLabel _HeaderElement = new TextLabel() { HorizontalAlign = HorizontalAlignment.Center, VerticalAlign = VerticalAlignment.Center };
        bool _HasBeenPressed;
        public string Header
        {
            get => _HeaderElement.Text;
            set => _HeaderElement.Text = value;
        }
        public Color Foreground
        {
            get => _HeaderElement.Foreground;
            set => _HeaderElement.Foreground = value;
        }
        public Font Font
        {
            get => _HeaderElement.Font;
            set => _HeaderElement.Font = value;
        }
        public int FontSize
        {
            get => _HeaderElement.FontSize;
            set => _HeaderElement.FontSize = value;
        }
        public bool IsMouseHovering { get; private set; }
        public bool IsDown { get; private set; }
        public Color DefaultBackground;
        public Color HoverBackground;
        public Color PressingBackground;
        public event Action OnActivated;

        public Button()
        {
            Foreground = Color.White;
            Background = DefaultBackground;
            FontSize = 9;
        }

        internal override void Update(Rectangle bounds)
        {
            UpdateBounds(bounds);
        
            IsDown = Input.MouseButtonsState(MouseButtons.Left).IsHolding;
            var mouseArea = new Rectangle
            (
                (int)(_Bounds.X * Game.GUI.Scale),
                (int)(_Bounds.Y * Game.GUI.Scale),
                (int)(_Bounds.Width * Game.GUI.Scale),
                (int)(_Bounds.Height * Game.GUI.Scale)
            );
            IsMouseHovering = mouseArea.Contains(Input.MouseGetPosition());

            if (IsMouseHovering)
            {
                Background = HoverBackground;
                if (IsDown)
                {
                    Background = PressingBackground;
                    if (!_HasBeenPressed)
                        _HasBeenPressed = true;
                }
                else if (_HasBeenPressed)
                {
                    OnActivated.Invoke();
                    
                    _HasBeenPressed = false;   
                }
            }
            else if (!IsDown)
            {
                _HasBeenPressed = false;
                Background = DefaultBackground;
            }

            _HeaderElement.Update(_Bounds);

            foreach (var element in Content)
            {
                Update(_Bounds);
            }
        }
        internal override void Render(RenderHelper render)
        {
            RenderPanelBackground(render);

            _HeaderElement.Render(render);
            
            RenderContent(render);
        }

    }
    
}
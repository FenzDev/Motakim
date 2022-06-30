using System;
using System.Collections.Generic;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Motakim
{
    public sealed class Font : IAsset
    {
        static Font()
        {
            _DefaultFont = new Font();
            var fontFile = new _FontFile()
            {
                DefaultCharacter = (int)'?',
                Fonts = new string[] 
                {
                    "Arial.ttf"
                }
            };
            _DefaultFont.Load(fontFile); 
        }
        internal static Font _DefaultFont = new Font();
        internal Dictionary<int, DynamicSpriteFont> _DynamicFonts = new Dictionary<int, DynamicSpriteFont>();
        internal FontSystem _FontSystem = new FontSystem();
        
        public static Font DefaultFont => _DefaultFont;
        public string Name { get; set; }
        public int? DefaultCharacter
        {
            get => _FontSystem.DefaultCharacter;
            set => _FontSystem.DefaultCharacter = value;
        } 

        private void Load(_FontFile file)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            foreach (var font in file.Fonts)
            {
                var fontFile = Path.Combine(folder, font);
                if (File.Exists(fontFile))
                {
                    _FontSystem.AddFont(File.ReadAllBytes(fontFile));
                }
                else
                {
                    _FontSystem.AddFont(TitleContainer.OpenStream(font));
                }
            }

            DefaultCharacter = file.DefaultCharacter;
        }
        
        internal DynamicSpriteFont GetFont(int fontSize)
        {
            if (_DynamicFonts.ContainsKey(fontSize))
            {
                return _DynamicFonts[fontSize];
            }
            else
            {
                var font = _FontSystem.GetFont(fontSize);
                _DynamicFonts.Add(fontSize, font);
                return font;
            }
        }

        public void Dispose()
        {
            _FontSystem = null;
            _DynamicFonts = null;
        }

        public void Load(Stream stream)
        {
            var serial = new JsonSerializer();
            var reader = new JsonTextReader(new StreamReader(stream));

            Load( serial.Deserialize<_FontFile>(reader));

        }

        public Vector2 MeasureText(int fontSize, string text)
        {
            return GetFont(fontSize).MeasureString(text);
        }

        class _FontFile
        {
            public int DefaultCharacter;
            public string[] Fonts;
        }
    }
}
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using XnaSoundEffect = Microsoft.Xna.Framework.Audio.SoundEffect;

namespace Motakim
{
    public class SoundEffect : IAsset
    {
        public SoundEffect() {}

        internal XnaSoundEffect _SFX; 
        public string Name { get; private set; } = "SFX";
        public bool IsDisposed => _IsDisposed;
        protected bool _IsDisposed;

        public static SoundEffectInstance Play(SoundEffect sfx, float volume) => Play(sfx, volume, 1f, false, 0f);
        public static SoundEffectInstance Play(SoundEffect sfx, float volume, float pitch) => Play(sfx, volume, pitch, false, 0f);
        public static SoundEffectInstance Play(SoundEffect sfx, float volume, float pitch, bool loop, float pan)
        {
            var instance = sfx.CreateInstance();
            
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.IsLooped = loop;
            instance.Pan = pan;

            return instance;
        }
        
        internal XnaSoundEffect GetSoundEffect()
        {
            if (_SFX == null)
            {
                _SFX = new XnaSoundEffect(new byte[0], 0, AudioChannels.Mono);
            }

            return _SFX;
        }
        public void Load(Stream stream)
        {
            if (_SFX != null) _SFX.Dispose();

            _SFX = XnaSoundEffect.FromStream(stream);
            
            Name = _SFX.Name;
        }
        public void Dispose()
        {
            if (_SFX.Equals(null)) _SFX.Dispose();
            _IsDisposed = true;
        }
        public SoundEffectInstance CreateInstance()
        {
            return new SoundEffectInstance(this);
        }

        public override string ToString() => Name;
    }
}
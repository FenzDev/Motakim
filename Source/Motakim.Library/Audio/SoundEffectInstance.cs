using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using XnaSoundEffectInstance = Microsoft.Xna.Framework.Audio.SoundEffectInstance;
using XnaSoundEffect = Microsoft.Xna.Framework.Audio.SoundEffect;

namespace Motakim
{
    public class SoundEffectInstance
    {
        private XnaSoundEffectInstance _SFXInstance;
        public SoundEffect SoundEffect { get; }
        public float Pan
        {
            get => _SFXInstance.Pan;
            set => _SFXInstance.Pan = value;
        }
        public float Volume
        {
            get => _SFXInstance.Volume;
            set => _SFXInstance.Volume = value;
        }
        public float Pitch
        {
            get => _SFXInstance.Pitch;
            set => _SFXInstance.Pitch = value;
        }
        public bool IsLooped
        {
            get => _SFXInstance.IsLooped;
            set => _SFXInstance.IsLooped = value;
        }

        internal SoundEffectInstance(SoundEffect sfx)
        {
            SoundEffect = sfx;
            _SFXInstance = sfx._SFX.CreateInstance();
        }
        public void Play() => _SFXInstance.Play();
        public void Pause() => _SFXInstance.Pause();
        public void Stop() => _SFXInstance.Stop();
    }

    public enum SoundState
    {
        Playing,
        Paused,
        Stop
    }
}
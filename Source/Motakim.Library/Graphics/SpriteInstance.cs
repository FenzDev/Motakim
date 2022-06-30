namespace Motakim
{
    public sealed class SpriteInstance
    {
        private int _TicksPassed;
        public bool IsPlaying { get; private set; }
        public Sprite Sprite { get; private set; }
        public int CurrentIndex;
        public SpriteFrame CurrentFrame => Sprite.Frames[CurrentIndex];
        
        internal SpriteInstance(Sprite sprite, int frameIndex, bool startAnimation)
        {
            Sprite = sprite;
            CurrentIndex = frameIndex;
            IsPlaying = startAnimation;
        }
        
        internal void Update()
        {
            if (IsPlaying)
            {
                if (++_TicksPassed > Sprite.FrameDuration)
                {
                    var frames = Sprite.Frames;
                    
                    _TicksPassed = 0;
                    
                    if (++CurrentIndex > frames.Length - 1)
                    {
                        CurrentIndex = 0;
                    }
                }
            }

        }
        public void Play()
        {
            IsPlaying = false;
        }
        public void Pause()
        {
            IsPlaying = false;
        }
        public void Stop()
        {
            Pause();
            _TicksPassed = 0;
            CurrentIndex = 0;
        }
    }
}
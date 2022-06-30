using System;

namespace Motakim
{
    public abstract class Component : ICloneable, IComponent
    {
        public bool Enabled { get; set; } = true;
        public Entity Entity { get; internal set; }

        public virtual void OnAdded() {}
        public virtual void OnRemoved() {}
        public virtual void OnEntityAdded() {}
        public virtual void OnEntityRemoved() {}

        public virtual object Clone() 
        {
            return MemberwiseClone();
        }
    }
}

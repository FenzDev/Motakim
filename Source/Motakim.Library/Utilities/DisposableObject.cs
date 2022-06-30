using System;

namespace Motakim.Utilities {
    public class DisposableObject : IDisposable
    {
        private bool _IsDisposed;
        public virtual bool IsDisposed => _IsDisposed; 

        protected void ValidateWhetherDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException(ToString());
        }
        public virtual void Dispose()
        {
            _IsDisposed = false;
        }
    }
}

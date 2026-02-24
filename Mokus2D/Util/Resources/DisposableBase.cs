using System;

namespace Mokus2D.Util.Resources
{
    public class DisposableBase : IDisposable
    {
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        ~DisposableBase()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(false);
            }
        }

        private bool disposed;
    }
}

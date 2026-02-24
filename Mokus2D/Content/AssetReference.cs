using System;

namespace Mokus2D.Content
{
    internal class AssetReference(object asset) : IDisposable
    {
        public void Dispose()
        {
            if (Asset is IDisposable)
            {
                ((IDisposable)Asset).Dispose();
            }
        }

        public object Asset = asset;

        public int ReferencesCount = 0;
    }
}

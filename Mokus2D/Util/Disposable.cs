using System;

using Mokus2D.Util.Resources;

namespace Mokus2D.Util
{
    public class Disposable(Action action) : DisposableBase
    {
        protected override void Dispose(bool disposing)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }
    }
}

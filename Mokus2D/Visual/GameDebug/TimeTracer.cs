using System;

namespace Mokus2D.Visual.GameDebug
{
    public class TimeTracer : IDisposable
    {
        public void Dispose()
        {
            Mokus2D.Util.DebugLog.infoFmt("elapsed time {0}", [DateTime.Now - start]);
        }

        private DateTime start = DateTime.Now;
    }
}


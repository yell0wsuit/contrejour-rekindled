using System;

namespace Mokus2D.Visual.GameDebug
{
    public struct GarbageTracer : IDisposable
    {
        public GarbageTracer(object argument, bool start = true)
        {
            this = default(GarbageTracer);
            this.argument = argument;
            if (start)
            {
                Start();
            }
        }

        public readonly void Start()
        {
        }

        public readonly void End()
        {
        }

        public void Dispose()
        {
            End();
        }

        private const string INFO_MESSAGE_ARG = "Garbage Generation in {0} {1} bytes";

        private object argument;

        private long memory;
    }
}

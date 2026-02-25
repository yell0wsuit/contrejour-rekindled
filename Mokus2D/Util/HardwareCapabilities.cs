using System;

using Microsoft.Phone.Info;

namespace Mokus2D.Util
{
    public static class HardwareCapabilities
    {
        public static bool IsLowMemoryDevice
        {
            get
            {
                if (!lowMemoryChecked)
                {
                    try
                    {
                        long applicationMemoryLimit = ApplicationMemoryLimit;
                        isLowMemory = applicationMemoryLimit < 94371840L;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        isLowMemory = false;
                    }
                    lowMemoryChecked = true;
                }
                return isLowMemory;
            }
        }

        private static long ApplicationMemoryLimit => (long)DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit");

        private const long NormalMemorySize = 94371840L;

        private static bool isLowMemory;

        private static bool lowMemoryChecked;
    }
}

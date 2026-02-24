using System;

namespace Microsoft.Phone.Info
{
    public static class DeviceExtendedProperties
    {
        public static object GetValue(string propertyName)
        {
            return string.Equals(propertyName, "ApplicationWorkingSetLimit", StringComparison.Ordinal) ? 188743680L : 0L;
        }
    }
}

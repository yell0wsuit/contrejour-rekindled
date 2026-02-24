using Microsoft.Xna.Framework;

namespace Mokus2D.Util
{
    public static class XNAExtensions
    {
        public static bool IsLandscape(this DisplayOrientation orientation)
        {
            return orientation == DisplayOrientation.LandscapeLeft || orientation == DisplayOrientation.LandscapeRight;
        }
    }
}

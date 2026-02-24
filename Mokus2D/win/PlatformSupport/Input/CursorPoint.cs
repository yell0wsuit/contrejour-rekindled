using Microsoft.Xna.Framework;

namespace Mokus2D.win.PlatformSupport.Input
{
    public struct CursorPoint(Vector2 position, int id)
    {
        public readonly Vector2 Position = position;

        public readonly int Id = id;
    }
}

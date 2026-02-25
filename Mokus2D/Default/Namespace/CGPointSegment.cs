using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class CGPointSegment
    {
        public CGPointSegment()
        {
            A = Vector2.Zero;
            B = Vector2.Zero;
        }

        public CGPointSegment(Vector2 A, Vector2 B)
        {
            this.A = A;
            this.B = B;
        }

        public Vector2 A = default(Vector2);

        public Vector2 B = default(Vector2);
    }
}

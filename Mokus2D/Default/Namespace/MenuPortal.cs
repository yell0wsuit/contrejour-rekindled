using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class MenuPortal(Vector2 position) : Portal(null, position)
    {
        public override void Update(float time)
        {
            base.Update(time);
            foreach (Satellite satellite in parts)
            {
                satellite.Update(time);
            }
        }
    }
}

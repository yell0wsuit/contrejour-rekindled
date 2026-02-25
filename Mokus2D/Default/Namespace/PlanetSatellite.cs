using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class PlanetSatellite : Node, IUpdatable
    {
        public PlanetSatellite()
        {
            satellite = ClipFactory.CreateWithAnchor("McSatellite");
            changer = new CosChanger(0.03f, 0.035f)
            {
                MinValue = CocosUtil.iPadValue(-150f),
                MaxValue = CocosUtil.iPadValue(150f)
            };
            AddChild(satellite);
        }

        public override void Update(float time)
        {
            changer.Update(time);
            satellite.Position = new Vector2(changer.Value, 0f);
            satellite.Scale = changer.GetValue(0.5f, 1f, changer.Progress - 1.5707964f);
            satellite.OpacityFloat = satellite.Scale;
            Rotation += 20f * time;
            int num = (satellite.Scale < 0.75f) ? (-1) : 1;
            Parent.ChangeChildLayer(this, num);
        }

        private const float ROTATION_STEP = 20f;

        private const float RADIUS = 150f;

        protected Sprite satellite;

        protected CosChanger changer;
    }
}

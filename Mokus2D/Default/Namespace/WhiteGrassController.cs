using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class WhiteGrassController(PlasticinePartBodyClip _plasticine) : GrassController(_plasticine)
    {
        public override float SmallGrassScale => 0.7f;

        public override int GrassFrame => Maths.Random(3);

        public override int SmallGrassFrame => GrassFrame;

        public float SmallGrassOffset(int index)
        {
            return ((index * 2) - 1) * plasticine.Width / 3f;
        }

        public override float WindAngle => 0.3926991f;

        public override float TrampleAngle => 0.2617994f;

        public override float SmallGrassStep => base.SmallGrassStep / 2f;

        public override float GrassStep => base.GrassStep / 2f;

        public override void Update(float time)
        {
            base.Update(time);
            if (!borderUpdated)
            {
                if (plasticine.Item.PreviousItem.BodyClip.GrassController == null)
                {
                    GrassAndPosition grassAndPosition = smallGrasses[0];
                    Vector2 vector = new(grassAndPosition.Position.X, -7f * builder.EngineConfig.SizeMultiplier);
                    grassAndPosition.Position = vector;
                    grassAndPosition.Particle.Scale = 0.4f;
                }
                if (plasticine.Item.NextItem.BodyClip.GrassController == null)
                {
                    GrassAndPosition grassAndPosition2 = smallGrasses[1];
                    Vector2 vector2 = new(grassAndPosition2.Position.X, -7f * builder.EngineConfig.SizeMultiplier);
                    grassAndPosition2.Position = vector2;
                    grassAndPosition2.Particle.Scale = 0.4f;
                }
                borderUpdated = true;
            }
        }

        public override float GetSmallGrassOffset(int index)
        {
            return index != 0 ? plasticine.Width / 2f : -plasticine.Width / 2f;
        }

        private const float BORDER_OFFSET = -7f;

        private const int WHITE_GRASS_COUNT = 3;

        protected bool borderUpdated;
    }
}

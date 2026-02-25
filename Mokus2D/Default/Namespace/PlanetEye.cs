using System;

using ContreJourMono.ContreJour.Game.Eyes;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class PlanetEye : BackSnotEye, IVectorPositionProvider
    {
        public PlanetEye(ContreJourGame _game, bool _visible, Vector2 position)
            : base(_game, _visible, position)
        {
            eyePosition = Vector2.Zero;
            ClipFactory.Cache("McPlanetEyeBlink");
            ClipFactory.Cache("McPlanetEyeBlinkOneTime");
            UpdateEnabled = true;
        }

        private string MaskName()
        {
            return null;
        }

        protected override EyeAnimation[] Animations => [
                    new EyeAnimation("McPlanetEyeBlink", null, false, false),
                    new EyeAnimation("McPlanetEyeBlinkOneTime", null, false, false)
                ];

        public override float EyeStep => 0.25f;

        protected override float ViewRadius => CocosUtil.iPadValue(25f);

        public override void Update(float time)
        {
            base.Update(time);
            Vector2 vector = speed;
            vector *= time;
            eyePosition += vector;
        }

        public Vector2 PositionVec => eyePosition;

        protected override void ScheduleAnimation()
        {
            _ = Schedule(new Action(Animate), Maths.randRange(3f, 10f));
        }

        protected override void ChangePositionProvider()
        {
            base.ChangePositionProvider();
            float num = Maths.randRange(-10f, 10f);
            float num2 = Maths.randRange(-MaxAngle(), MaxAngle()) - 0.7853982f;
            eyePosition = FarseerUtil.ToVecAngle(num, num2);
            positionProvider = this;
            float num3 = Maths.Random(2f);
            float num4 = num2 + 3.1415927f;
            speed = FarseerUtil.ToVecAngle(num3, num4);
        }

        protected virtual float MaxAngle()
        {
            return 0.2617994f;
        }

        protected override void CreateDefaultView()
        {
            background = ClipFactory.CreateWithAnchor("McPlanetEye");
            eyeBall = ClipFactory.CreateWithAnchor("McPlanet1EyeBall");
            eyeBall.Scale = 1.15f;
        }

        private const float MAX_TIMEOUT = 10f;

        private const float MIN_TIMEOUT = 3f;

        private const float MAX_ANGLE = 0.2617994f;

        private const float MAX_OFFSET = 10f;

        private const float MAX_SPEED = 2f;

        protected Vector2 eyePosition;

        protected Vector2 speed;
    }
}

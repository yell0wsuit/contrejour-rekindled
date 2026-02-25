using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class MoveCharHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config) : FadeHint(_builder, _body, _clip, _config)
    {
        public override bool HasToHide()
        {
            return false;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!initialPositionSet && contreJour.Hero != null)
            {
                initialPosition = contreJour.HeroPositionPixels;
                initialPositionSet = true;
                return;
            }
            if (!hiding && Maths.Abs(contreJour.HeroPositionPixels.X - initialPosition.X) > CocosUtil.iPadValue(60f))
            {
                Hide();
            }
        }

        private const float MAX_DISTANCE = 60f;

        protected Vector2 initialPosition;

        protected bool initialPositionSet;
    }
}

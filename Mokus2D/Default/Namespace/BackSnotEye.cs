using ContreJourMono.ContreJour.Game.Eyes;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class BackSnotEye : MonsterEye
    {
        public BackSnotEye(ContreJourGame _game, bool _visible, Vector2 position)
            : base(_game, _visible, position)
        {
            eyeStep = CocosUtil.iPadValue(3f);
        }

        protected override EyeAnimation[] Animations => BACK_SNOT_ANIMATIONS;

        protected override float ViewRadius => CocosUtil.iPadValue(30f);

        public override void Update(float time)
        {
            base.Update(time);
            currentBackground.Position = currentEyeBall.Position * 0.4f;
        }

        protected override void CreateDefaultView()
        {
            background = ClipFactory.CreateWithAnchor("McBackSnotEye");
            eyeBall = ClipFactory.CreateWithAnchor("McBackSnotEyeBall");
        }

        public static readonly EyeAnimation[] BACK_SNOT_ANIMATIONS =
        [
            new EyeAnimation("McBackSnotEyeBlink", null, false, false),
            new EyeAnimation("McBackSnotEyeBlinkOneTime", null, false, false)
        ];
    }
}

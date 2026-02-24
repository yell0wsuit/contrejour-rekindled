using System;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;

namespace Default.Namespace
{
    public class HeroExplosion
    {
        public void ExplodeGame(HeroBodyClip _bodyClip, ContreJourGame _game)
        {
            bodyClip = _bodyClip;
            bodyClip.Body.BodyType = BodyType.Static;
            game = _game;
            NodeAction nodeAction = null;
            HeroEye eye = bodyClip.Eye;
            eye.SetDefaultView();
            eye.AnimationsAllowed = false;
            bodyClip.Clip.Scale = bodyClip.Clip.ScaleX;
            for (int i = 0; i < 15; i++)
            {
                Vector2 vector = CocosUtil.ccpIPad(Maths.randRange(-2f, 2f), Maths.randRange(-2f, 2f));
                vector += bodyClip.Clip.Position;
                MoveTo moveTo = new(0.02f, vector);
                float num = 1f + i / 15f / 5f;
                num += ((i % 2 != 0) ? 0.05f : (-0.05f));
                ScaleTo scaleTo = new(0.02f, num);
                Spawn spawn = new([moveTo, scaleTo]);
                if (nodeAction == null)
                {
                    nodeAction = spawn;
                }
                else
                {
                    nodeAction = new Sequence([nodeAction, spawn]);
                }
            }
            InstantAction instantAction = new(new Action(DoExplode));
            nodeAction = new Sequence([nodeAction, instantAction]);
            bodyClip.Clip.Run(nodeAction);
        }

        private void DoExplode()
        {
            explosion = new Explosion(game.BlackSide ? "McHeroSmokeBlack.png" : "McWhiteSmoke.png");
            if (game.BonusChapter)
            {
                explosion.Color = ContreJourConstants.GreenLightColor;
            }
            explosion.Position = bodyClip.Clip.Position;
            explosion.HorizontalPosition = new Range(0f, 5f);
            explosion.VerticalPosition = new Range(0f, 5f);
            explosion.CreateOnStartPosition(14);
            bodyClip.Builder.Add(explosion, 10);
            bodyClip.DoExplode();
        }

        private const float SHAKE_OFFSET = 2f;

        private const int SHAKE_COUNT = 15;

        protected Explosion explosion;

        protected HeroBodyClip bodyClip;

        protected ContreJourGame game;
    }
}

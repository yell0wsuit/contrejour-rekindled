using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SpikesFlowerBodyClip : ContreJourBodyClip, IVectorPositionProvider
    {
        public FlowerEye Eye => eye;

        public SpikesFlowerBodyClip(ContreJourLevelBuilder _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            if (Game.WhiteSide || Game.BonusChapter)
            {
                Node node = _clip;
                ContreJourGame game = Game;
                string text = "McSpikesViewWhite";
                string text2 = "McSpikesView_6";
                _clip = _builder.ReplaceClipWith(node, game.Choose(null, null, text, null, text2));
                clip = _clip;
            }
            container = new Node();
            builder.AddChildBefore(container, clip);
            container.Position = _clip.Position;
            container.Rotation = _clip.Rotation;
            Node node2 = ClipFactory.CreateWithAnchor("McSpikesFlowerShadow");
            node2.Scale = clip.ScaleY;
            container.AddChild(node2);
            movie = (MovieClip)_clip;
            movie.Stoped = true;
            movie.Speed = 1.5f;
            drawing = new SpikesFlowerSprite(this, movie.ScaleY);
            container.AddChild(drawing, -1);
        }

        private void CreateEye()
        {
            Vector2 vector = container.LocalToNode(EYE_POSITION, Game.Root, true);
            eye = new FlowerEye(Game, true, builder.ToVec(vector))
            {
                Scale = movie.ScaleY * 0.7f
            };
            container.AddChild(eye);
            eye.RefreshRootAngle();
            eye.Position = CocosUtil.toIPad(EYE_POSITION);
        }

        public override Vector2 PositionVec => Body.Position;

        public override void Update(float time)
        {
            base.Update(time);
            if (eye == null && container.Root != null)
            {
                CreateEye();
            }
            movie.Update(time);
            eye.UpdateNode(time);
            drawing.Update(time);
        }

        public override void OnCollisionPoint(Body body2, Contact point)
        {
            IEatable eatable = body2.UserData as IEatable;
            if (eatable != null && movie.Stoped && Maths.FuzzyEquals(movie.CurrentFrame, 0f, 0.0001f) && eatable.CanDie())
            {
                hero = eatable;
                hero.EatSpeedPauseScaleTime(Body.Position, 0.5f, 1.3f, 0f, 0.2f);
                if (hero is HeroBodyClip)
                {
                    UserData instance = UserData.Instance;
                    instance.FeedMonster++;
                }
                hero.Clip.Parent.ChangeChildLayer(hero.Clip, -1);
                movie.Stoped = false;
                movie.Rewind = false;
                movie.EndEvent += new Action(OnCloseEnd);
                eye.PositionProvider = this;
                Mokus2DGame.SoundManager.PlaySound("deathByFlowerOut4", 0.5f, 0f, 0f);
            }
        }

        private void OnCloseEnd()
        {
            movie.EndEvent -= new Action(OnCloseEnd);
            movie.GotoAndStop(movie.TotalFrames - 1U);
            _ = Schedule(new Action(Open), 1f);
        }

        private void Open()
        {
            Mokus2DGame.SoundManager.PlaySound("deathByFlowerOut10", 0.7f, 0f, 0f);
            movie.Rewind = true;
            movie.Stoped = false;
            movie.Repeat = false;
            eye.PositionProvider = null;
            CreateDeadEye();
        }

        public void CreateDeadEye()
        {
            Body body = FarseerUtil.CreateCircle(builder.World, 16f * builder.EngineConfig.SizeMultiplier * hero.DeadEyeScale(), Body.Position, 0f, builder.EngineConfig.Density, true);
            Node node = ClipFactory.CreateWithAnchor((Game.WhiteSide || Game.BonusChapter) ? "McEyeDeadBlack" : "McEyeDead");
            if (Game.BonusChapter)
            {
                node.Color = ContreJourConstants.GreenLightColor;
            }
            BodyClip bodyClip = new(builder, body, node, null);
            builder.Add(node, 10);
            node.Scale = 0f;
            ScaleTo scaleTo = new(0.3f, hero.DeadEyeScale());
            Sequence sequence = new(
            [
                scaleTo,
                new FadeOut(2f)
            ]);
            node.Run(sequence);
            _ = Schedule(new Action(bodyClip.Destroy), 2.3f);
            Vector2 vector = FarseerUtil.rotate(new Vector2(Maths.randRange01(), 3f), BodyAngle);
            vector *= (float)Math.Pow((double)hero.DeadEyeScale(), 2.0);
            body.ApplyLinearImpulse(vector, body.WorldCenter);
        }

        private const float EYE_SCALE = 0.7f;

        private const float DEAD_EYE_RADIUS = 16f;

        protected IEatable hero;

        protected MovieClip movie;

        protected FlowerEye eye;

        protected SpikesFlowerSprite drawing;

        protected Node container;

        private static readonly Vector2 EYE_POSITION = new(0f, 30f);
    }
}

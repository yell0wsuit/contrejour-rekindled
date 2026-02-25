using System;

using ContreJourMono.ContreJour.Game.Eyes;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BackSnotBodyClip : SnotBodyClipBase, IClickable
    {
        public BackSnotBodyClip(LevelBuilderBase _builder, SnotData _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            force = 0.25f;
            forceProgress = Maths.RandRangeMinMax(0f, 6.2831855f);
            forceStep = Maths.RandRangeMinMax(0.01f, 0.02f);
            stabilize = false;
            stabilizeCalculated = false;
            Vector2 vector = config.GetVector("scale");
            eye.Scale = vector.X / 11.24f;
            baseClip.Scale = eye.Scale;
            baseEndClip.Scale = eye.Scale;
        }

        public override Body EyeBody => Physics.EndBody;

        public bool DisableHeroFocus => true;

        public int Priority(Vector2 touchPoint)
        {
            return -1;
        }

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public bool UseForZoom()
        {
            return true;
        }

        public bool TouchBegan(Touch touch)
        {
            Vector2 vector = Physics.GetWorldStartPoint() - Physics.EndBody.Position;
            vector *= 0.7f / vector.Length();
            Physics.EndBody.ApplyLinearImpulse(vector, Physics.EndBody.WorldCenter);
            eye.PlayAnimation(new EyeAnimation("McBackSnotEyeBlink", null, false, false), false);
            eye.RandomPositionProvider = game.GetTouchProvider(touch);
            Mokus2DGame.SoundManager.PlayRandomSound(Sounds.BACK_SNOT, Maths.randRange(0.3f, 0.5f));
            if (Maths.Rand() < 0.5f)
            {
                Mokus2DGame.SoundManager.PlaySound("backgroundEyeHit0", Maths.randRange(0.3f, 0.5f), 0f, 0f);
            }
            return false;
        }

        public bool TouchMove(Touch touch)
        {
            return false;
        }

        public void TouchOut(Touch touch)
        {
        }

        public void TouchEnd(Touch touch)
        {
        }

        public override int Layer()
        {
            return -9;
        }

        public override string BaseClipName()
        {
            return "McBackSnotBase";
        }

        public override string BaseEndClipName()
        {
            return "McBackSnotBase";
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (eye != null)
            {
                if (!stabilizeCalculated)
                {
                    stabilizeCalculated = true;
                    float num = (float)Math.Ceiling((double)(Maths.SimplifyAngleStartValue(rotationOffset, -180f) / 90f));
                    stabilize = Maths.FuzzyNotEquals(num, 0f, 0.0001f);
                }
                for (int i = 0; i < Physics.BodiesSize(); i++)
                {
                    Body body = Physics.BodyAt(i);
                    body.GravityScale = stabilize ? 0 : 1;
                }
                Physics.EndBody.GravityScale = stabilize ? 0.2f : 0f;
                float num2 = (force / 4f * 3f) + (force * Maths.Cos(forceProgress) / 4f);
                forceProgress += forceStep;
                Physics.EndBody.ApplyForce(FarseerUtil.ToVecAngle(num2, eye.ViewAngle), Physics.EndBody.WorldCenter);
            }
        }

        public override SnotSprite CreateClip()
        {
            Vector2 vector = config.GetVector("scale");
            return new BackSnotSprite(this, 9f * vector.X * builder.EngineConfig.SizeMultiplier, 4f * vector.X * builder.EngineConfig.SizeMultiplier, 9f * vector.X * builder.EngineConfig.SizeMultiplier)
            {
                NeckColor = new Color(60, 60, 60, 255)
            };
        }

        protected override MonsterEye CreateEye()
        {
            return new BackSnotEye((ContreJourGame)builder.Game, true, Physics.EndBody.Position);
        }

        private const float SCARE_FORCE = 0.7f;

        private const float BASE_SCALE = 11.24f;

        protected float force;

        protected float forceProgress;

        protected float forceStep;

        protected bool stabilize;

        protected bool stabilizeCalculated;
    }
}

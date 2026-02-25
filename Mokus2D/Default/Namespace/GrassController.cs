using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class GrassController : IGrassController, IUpdatable
    {
        public Particle Grass => grass;

        public virtual float Y => grass.Position.Y;

        public virtual int GrassFrame => Maths.Random(8);

        public virtual float WindAngle => 0.5235988f;

        public virtual int SmallGrassFrame => Maths.Random(5) + 8;

        public virtual float SmallGrassScale => 0.5f;

        public virtual float GetSmallGrassOffset(int index)
        {
            return Maths.RandRangeMinMax(-plasticine.Width, plasticine.Width);
        }

        public virtual float TrampleAngle => 0.5235988f;

        public virtual float SmallGrassStep => !touched ? 1f : 4f;

        public virtual float GrassStep => !touched ? 1f : 2.5f;

        public GrassController(PlasticinePartBodyClip _plasticine)
        {
            plasticine = _plasticine;
            builder = (ContreJourLevelBuilder)plasticine.Builder;
            game = (ContreJourGame)plasticine.Builder.Game;
            startAngle = Maths.RandRangeMinMax(-0.2617994f, 0.2617994f);
            touched = false;
            touchDistance = 0f;
            notTouchedFrames = 0;
            Create();
        }

        public void Create()
        {
            int num = Maths.min(GrassFrame, game.Grass.Config.Frames - 1);
            grass = game.Grass.AddParticleWithFrame(num);
            grass.Position = builder.ToIPadPoint(plasticine.GetSurfaceCenter());
            RandomizeClipMinScaleMaxScale(grass, 0.45499998f, 0.65f);
            windData = new WindData(WindAngle);
            CreateSmallGrass();
            if (CocosUtil.isArmV7() && !plasticine.Parent.Config.GetBool("disableFlyes"))
            {
                CreateFlyes();
            }
        }

        public void CreateSmallGrass()
        {
            smallGrasses = new List<GrassAndPosition>();
            for (int i = 0; i < 3; i++)
            {
                int num = Maths.min(SmallGrassFrame, game.Grass.Config.Frames - 1);
                Particle particle = game.Grass.AddParticleWithFrame(num);
                RandomizeClipMinScaleMaxScale(particle, 0.6f * SmallGrassScale, SmallGrassScale);
                float smallGrassOffset = GetSmallGrassOffset(i);
                Vector2 vector = new(smallGrassOffset / builder.EngineConfig.SizeMultiplier, 2f);
                vector += grass.Position;
                particle.Position = vector;
                smallGrasses.Add(new GrassAndPosition(particle, new Vector2(smallGrassOffset, 0f)));
            }
        }

        public virtual void Update(float time)
        {
            if (!touched)
            {
                notTouchedFrames++;
            }
            UpdateGrassRotation(time);
            UpdateGrassPosition();
            foreach (object obj in flyes)
            {
                FlyController flyController = (FlyController)obj;
                flyController.Update(time);
            }
        }

        public void UpdateGrassRotation(float time)
        {
            float wind = game.WindManager.GetWind(windData.WindOffset);
            float num = plasticine.Body.Rotation + windData.GetAngle(wind) + startAngle;
            float num2 = num;
            if (notTouchedFrames >= 5)
            {
                touchDistance = 0f;
                touchingObject = null;
            }
            if (touchingObject != null)
            {
                float num3 = touchOffset - touchStartOffset;
                float num4 = 2.6666667f;
                if (num3 / touchStartOffset <= 0f && Maths.Abs(num3) < num4)
                {
                    touchDistance = Math.Sign(num3) * Math.Min(Maths.Abs(num3 / 1.3333334f), 1f);
                    float num5 = -Math.Min(num3 / 2f, 1f) * TrampleAngle;
                    num += num5 * 3f / 2f;
                    num2 += num5 * 2f;
                }
                else
                {
                    touchingObject = null;
                    touchDistance = 0f;
                }
            }
            float num6 = Maths.ToDegrees(num);
            float num7 = time * 30f;
            float smallGrassStep = SmallGrassStep;
            float grassStep = GrassStep;
            smallGrassRotation = Maths.StepToTargetMaxStep(smallGrassRotation, Maths.ToDegrees(num2), smallGrassStep * num7);
            grass.Rotation = Maths.StepToTargetMaxStep(grass.Rotation, num6, grassStep * num7);
            touched = false;
        }

        public void UpdateGrassPosition()
        {
            Vector2 surfaceCenter = plasticine.GetSurfaceCenter();
            grass.Position = builder.ToIPadPoint(surfaceCenter);
            foreach (GrassAndPosition grassAndPosition in smallGrasses)
            {
                Vector2 vector = grassAndPosition.Position + plasticine.GetLocalSurfaceCenter();
                Vector2 worldPoint = plasticine.Body.GetWorldPoint(vector);
                grassAndPosition.Particle.Position = builder.ToIPadPoint(worldPoint);
                grassAndPosition.Particle.Rotation = builder.ToRotation(plasticine.Body.Rotation);
                grassAndPosition.Particle.Rotation = smallGrassRotation;
            }
        }

        public void CreateFlyes()
        {
            flyes = new ArrayList();
            int num = (game.RoseChapter || game.BonusChapter) ? 1 : 2;
            for (int i = 0; i < num; i++)
            {
                Vector2 vector = new(Maths.RandRangeMinMax(-plasticine.Width, plasticine.Width), Maths.RandRangeMinMax(1.3333334f, 2f));
                vector += plasticine.Body.Position;
                Particle particle = game.Flyes.AddParticle(builder.ToIPadPoint(vector));
                flyes.Add(new FlyController(game, plasticine, particle));
            }
        }

        public void OnTouchWith(float offset, BodyClip objectP)
        {
            ScareFlyes(offset);
            if (touchingObject == null || (objectP != touchingObject && Maths.Abs(offset) < Maths.Abs(touchOffset)))
            {
                touchStartOffset = Math.Sign(offset) * 1.3333334f;
                touchingObject = objectP;
            }
            if (touchingObject == objectP)
            {
                touchOffset = offset;
                notTouchedFrames = 0;
            }
            touched = true;
        }

        public void ScareFlyes(float offset)
        {
            if (Maths.FuzzyEquals(offset, 0f, 0.0001f))
            {
                offset = (Maths.Rand() > 0.5f) ? 1 : (-1);
            }
            foreach (object obj in flyes)
            {
                FlyController flyController = (FlyController)obj;
                flyController.Scare(-Math.Sign(offset));
            }
        }

        public void RandomizeClipMinScaleMaxScale(Particle _clip, float minScale, float maxScale)
        {
            _clip.Scale = Maths.RandRangeMinMax(minScale, maxScale);
        }

        private const int FLYES_COUNT_LAST_CHAPTER = 1;

        private const int FLYES_COUNT = 2;

        private const float SMALL_GRASS_ANGLE_DIFF = 4f;

        private const float GRASS_ANGLE_DIFF = 2.5f;

        private const float GRASS_ANGLE_DIFF_BACK = 1f;

        private const float FLY_MIN_OFFSET = 1.3333334f;

        private const float FLY_MAX_OFFSET = 2f;

        private const float TRAMPLE_ANGLE = 0.5235988f;

        private const int NO_TOUCH_FRAMES = 5;

        private const int SMALL_GRASS_ON_GROUND = 3;

        private const int SMALL_GRASS_COUNT = 5;

        private const float SMALL_GRASS_SCALE = 0.5f;

        private const float GRASS_SCALE = 0.65f;

        private const int GRASS_COUNT = 8;

        protected PlasticinePartBodyClip plasticine;

        protected ContreJourGame game;

        protected ContreJourLevelBuilder builder;

        protected Particle grass;

        protected List<GrassAndPosition> smallGrasses;

        protected WindData windData;

        protected ArrayList flyes;

        protected float startAngle;

        protected bool touched;

        protected float touchDistance;

        protected int notTouchedFrames;

        protected BodyClip touchingObject;

        protected float touchStartOffset;

        protected float touchOffset;

        protected float smallGrassRotation;
    }
}

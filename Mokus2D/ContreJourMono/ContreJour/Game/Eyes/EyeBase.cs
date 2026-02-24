using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace ContreJourMono.ContreJour.Game.Eyes
{
    public abstract class EyeBase : Node
    {
        public virtual float EyeStep
        {
            get
            {
                return eyeStep;
            }
            set
            {
                eyeStep = value;
            }
        }

        public Sprite CurrentBackground
        {
            get
            {
                return currentBackground;
            }
        }

        public float ViewDistance
        {
            get
            {
                return viewDistance;
            }
            set
            {
                if (value != viewDistance)
                {
                    viewDistance = value.Clamp(0f, 1f);
                    dirty = true;
                }
            }
        }

        public float ViewAngle
        {
            get
            {
                return viewAngle;
            }
            set
            {
                if (value != viewAngle)
                {
                    viewAngle = value;
                    dirty = true;
                }
            }
        }

        protected virtual float ViewRadius
        {
            get
            {
                return 7f;
            }
        }

        protected ContreJourGame Game
        {
            get
            {
                return game;
            }
        }

        public EyeBase(ContreJourGame game)
            : this(game, false, Vector2.Zero)
        {
        }

        public EyeBase(ContreJourGame game, bool useMask, Vector2 maskSize)
        {
            this.game = game;
            this.useMask = useMask;
            this.maskSize = maskSize;
            CreateDefaultView();
            if (useMask)
            {
                mask = new AnimatedMaskedSprite(maskSize)
                {
                    UpdateMask = false
                };
                mask.AddChild(content);
                AddChild(mask);
            }
            else
            {
                AddChild(content);
            }
            SetDefaultView();
        }

        protected virtual string ProcessName(string name)
        {
            return name;
        }

        protected MovieClip CreateEyeBall(EyeAnimation animation)
        {
            return Create(animation.EyeBall);
        }

        protected MovieClip CreateBackground(EyeAnimation animation)
        {
            return Create(animation.Background);
        }

        private MovieClip Create(string name)
        {
            return name == null ? null : (MovieClip)ClipFactory.CreateWithAnchor(ProcessName(name));
        }

        protected virtual void CreateDefaultView()
        {
            background = ClipFactory.CreateWithAnchor("McEye");
            eyeBall = ClipFactory.CreateWithAnchor("McEyeBall");
        }

        public void SetDefaultView()
        {
            SuspendLayout();
            if (currentBackground != null)
            {
                background.Position = currentBackground.Position;
            }
            if (currentEyeBall != null)
            {
                eyeBall.Position = currentEyeBall.Position;
            }
            endDispatcher = null;
            currentBackground = background;
            currentEyeBall = eyeBall;
            lockX = (lockY = false);
            RefreshLayout();
        }

        private void SuspendLayout()
        {
            if (currentEyeBall != null)
            {
                content.RemoveChild(currentEyeBall);
            }
            if (currentBackground != null)
            {
                content.RemoveChild(currentBackground);
            }
        }

        protected virtual void RefreshLayout()
        {
            if (currentBackground != null && currentBackground.Parent == null)
            {
                content.AddChild(currentBackground);
                if (useMask)
                {
                    mask.Mask = currentBackground;
                }
            }
            if (currentEyeBall != null && currentEyeBall.Parent == null)
            {
                content.AddChild(currentEyeBall);
            }
        }

        public void SetEyeContent(EyeAnimation animation)
        {
            SuspendLayout();
            lockX = animation.LockX;
            lockY = animation.LockY;
            endDispatcher = null;
            if (animation.ReplaceBackground)
            {
                MovieClip movieClip = CreateBackground(animation);
                movieClip.Position = currentBackground.Position;
                endDispatcher = movieClip;
                currentBackground = movieClip;
            }
            if (animation.ReplaceEye)
            {
                MovieClip movieClip2 = CreateEyeBall(animation);
                if (!animation.ReplaceBackground)
                {
                    endDispatcher = movieClip2;
                }
                currentEyeBall = movieClip2;
            }
            RefreshLayout();
        }

        public override void Update(float time)
        {
            if (dirty)
            {
                Refresh();
                dirty = false;
            }
            Vector2 vector = eyeBall.Position.StepTo(targetPosition, EyeStep);
            if (lockX)
            {
                vector.X = 0f;
            }
            if (lockY)
            {
                vector.Y = 0f;
            }
            currentEyeBall.Position = vector;
        }

        private void Refresh()
        {
            targetPosition = VectorUtil.ToVector(ViewRadius * ViewDistance, ViewAngle);
        }

        private const float STEP = 0.5f;

        private const float RADIUS = 7f;

        protected float eyeStep = 0.5f;

        protected Sprite background;

        protected Sprite eyeBall;

        protected MovieClip endDispatcher;

        protected Sprite currentBackground;

        protected Sprite currentEyeBall;

        private bool lockX;

        private bool lockY;

        private readonly bool useMask;

        private readonly Vector2 maskSize = new(50f, 50f);

        private float viewDistance;

        private float viewAngle;

        private Vector2 targetPosition = Vector2.Zero;

        private bool dirty = true;

        private readonly ContreJourGame game;

        private readonly AnimatedMaskedSprite mask;

        private readonly Node content = new();
    }
}

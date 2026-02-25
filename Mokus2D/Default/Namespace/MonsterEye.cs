using System;

using ContreJourMono.ContreJour.Game.Eyes;

using Microsoft.Xna.Framework;

using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class MonsterEye : RandomAnimationEye, IPositionDepedent
    {
        public IVectorPositionProvider PositionProvider
        {
            get => positionProvider; set => positionProvider = value;
        }

        public IVectorPositionProvider RandomPositionProvider
        {
            get => randomPositionProvider; set => randomPositionProvider = value;
        }

        public bool ProviderEnabled
        {
            get => providerEnabled; set => providerEnabled = value;
        }

        public MonsterEye(ContreJourGame _game, bool _visible, Vector2 position)
            : base(_game)
        {
            providerEnabled = true;
            Open = _visible;
            Visible = _visible;
            Schedule(new Action(ChangePositionProvider), 0.1f);
            if (Game != null)
            {
                Game.AddPositionDependent(this);
            }
            clipPosition = position;
            startAngle = 0f;
        }

        public void RefreshRootAngle()
        {
            startAngle = GraphUtil.GetRootRotationRadians(this);
        }

        public void ProviderAdded(IVectorPositionProvider provider)
        {
            if (Maths.Rand() < 0.5f)
            {
                ChangePositionProvider();
            }
        }

        public void ProviderRemove(IVectorPositionProvider _provider)
        {
            if (randomPositionProvider == _provider)
            {
                ChangePositionProvider();
            }
        }

        protected override void CreateDefaultView()
        {
            string text = (BlackEye ? "McEyeMonsterBlack" : "McEyeMonster");
            string text2 = Game.ChooseSide("McEyeBallMonsterBlack", "McEyeBallMonsterWhite", "McEyeBallMonster", "McEyeBallMonster", "McEyeBallMonster_6");
            background = ClipFactory.CreateWithAnchor(text);
            eyeBall = ClipFactory.CreateWithAnchor(text2);
        }

        protected override EyeAnimation[] Animations => SNOT_ANIMATIONS;

        public bool HasToUpdate => Visible;

        public bool Open
        {
            get => open;
            set
            {
                if (open != value)
                {
                    open = value;
                    if (value && !Visible)
                    {
                        AnimationEndEvent.RemoveListenerSelector(new Action(OnCloseEnd));
                        PlayAnimation(new EyeAnimation("McEyeOpenMonster", null, false, false), true);
                        Visible = true;
                        return;
                    }
                    if (!value && Visible)
                    {
                        PlayAnimation(new EyeAnimation("McEyeCloseMonster", null, false, false), true);
                        AnimationEndEvent.AddListenerSelector(new Action(OnCloseEnd));
                    }
                }
            }
        }

        protected virtual void ChangePositionProvider()
        {
            if (Game != null)
            {
                randomPositionProvider = Game.GetRandomPositionProvider();
            }
            StopAllActions();
            Schedule(new Action(ChangePositionProvider), Maths.RandRangeMinMax(3f, 15f));
        }

        public override void Update(float time)
        {
            if (providerEnabled)
            {
                IVectorPositionProvider vectorPositionProvider = positionProvider ?? randomPositionProvider;
                if (vectorPositionProvider != null)
                {
                    Vector2 vector = Vector2.Zero;
                    if (Game != null)
                    {
                        vector = Game.Builder.ToRootChild(Vector2.Zero, this);
                        clipPosition = Game.Builder.ToIPhoneVec(vector);
                    }
                    Vector2 vector2 = vectorPositionProvider.PositionVec - clipPosition;
                    ViewAngle = Maths.atan2(vector2.Y, vector2.X) - startAngle;
                    ViewDistance = Math.Min(vector2.Length() / 6.6666665f, 1f);
                }
            }
            base.Update(time);
            currentBackground.Position = currentEyeBall.Position * 0.5f;
        }

        private void OnCloseEnd()
        {
            AnimationEndEvent.RemoveListenerSelector(new Action(OnCloseEnd));
            Visible = false;
            if (Open)
            {
                Open = false;
                Open = true;
            }
        }

        private const float MAX_DISTANCE = 6.6666665f;

        private const float PROVIDER_MAX_TIME = 15f;

        private const float PROVIDER_MIN_TIME = 3f;

        protected Vector2 clipPosition;

        protected float startAngle;

        protected IVectorPositionProvider positionProvider;

        protected IVectorPositionProvider randomPositionProvider;

        protected bool providerEnabled;

        private bool open;

        public static readonly EyeAnimation[] SNOT_ANIMATIONS =
        [
            new EyeAnimation("McEyeBlinkMonster", null, false, false),
            new EyeAnimation("McEyeBlinkOneTimeMonster", null, false, false)
        ];
    }
}

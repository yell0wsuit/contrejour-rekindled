using System;
using System.Collections.Generic;

using ContreJourMono.ContreJour.Menu.LevelComplete;

using Default.Namespace.Windows.Items;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D;
using Mokus2D.Controls;
using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace.Windows
{
    public class XBoxWindow : PopUpWindow
    {
        public XBoxWindow()
        {
            Scale = 1f / ScreenConstants.Scales.fromIPhone2ByHeight;
            container.AddChild(new Sprite("McXBoxWindowBackground"));
            pictureBackground = new Sprite("McXboxUserImageBackground");
            container.AddChild(pictureBackground);
            pictureBackground.Position = new Vector2(135f, 369f);
            container.AddChild(scrollLayer);
            scrollLayer.Position = StartScrollPosition;
            Sprite sprite = new("McXboxWindowTitle")
            {
                Position = ScreenConstants.OsSizes.W7
            };
            container.AddChild(sprite);
            scrollLayer.RefreshBorders();
            container.AddChild(hero);
            hero.Position = new Vector2(pictureBackground.X, 127f);
            hero.Tail.UpdateSpeed = 0.7f;
            hero.Tail.Rotation = -10f;
            ScheduleBlink();
            _ = Schedule(new Action(ScheduleChangeView), 1f);
            scrollLayer.TouchBeginEvent.AddListenerSelector(new Action(OnScrollBegin));
        }

        private void OnScrollBegin()
        {
            if (Maths.Rand() < 0.5f && items.Count > 0)
            {
                _ = TryLookAtList();
            }
        }

        public override void Update(float time)
        {
            if (targetItem != null)
            {
                hero.LookAt(targetItem.ViewTarget);
            }
        }

        private void ScheduleBlink()
        {
            _ = Schedule(new Action(Blink), Maths.randRange(3f, 8f));
        }

        private void Blink()
        {
            hero.Eye.Blink();
            ScheduleBlink();
            if (Maths.Rand() < 0.3f)
            {
                ChangeView();
            }
        }

        private void ScheduleChangeView()
        {
            _ = Schedule(new Action(ScheduleChangeView), Maths.randRange(10f, 16f));
            ChangeView();
        }

        private bool TryLookAtList()
        {
            if (targetItem == null && items.Count > 0)
            {
                targetItem = items[Maths.Random(items.Count)];
                return true;
            }
            return false;
        }

        private void ChangeView()
        {
            if (!TryLookAtList())
            {
                if (Maths.Rand() < 0.5f && picture != null)
                {
                    hero.LookAt(picture);
                }
                else
                {
                    hero.LookAt(hero);
                }
                targetItem = null;
            }
        }

        protected void CreateTitle(string title)
        {
            titleLabel = ContreJourLabel.CreateLabel(16f, title, true);
            container.AddChild(titleLabel);
            titleLabel.Position = new Vector2(310f, 446f);
            titleLabel.AnchorX = 0f;
            titleLabel.Color = Color.White;
        }

        protected void ShowItem(Node item)
        {
            item.Opacity = 0;
            item.Run(new FadeIn(0.5f));
        }

        protected void AddItem(XBoxItem item)
        {
            item.Y = currentItemPosition;
            currentItemPosition -= item.Size.Y;
            scrollLayer.AddChild(item);
            if (Math.Abs(currentItemPosition) > scrollLayer.Position.Y)
            {
                scrollLayer.MaxPosition.Y = Math.Abs(scrollLayer.Position.Y + currentItemPosition) + StartScrollPosition.Y;
            }
            ShowItem(item);
            items.Add(item);
        }

        protected override void OnOpen()
        {
            if (!loading)
            {
                loading = true;
                loadingIcon = new Sprite("McLoadingIcon");
                container.AddChild(loadingIcon);
                loadingIcon.Position = new Vector2(520f, 245f);
                loadingIcon.Run(new RepeatForever(new RotateBy(40f, -314.15927f)));
                GetData();
            }
        }

        private void Close()
        {
            Open = false;
        }

        protected void Close(IAsyncResult result)
        {
            Close();
        }

        protected void SetNotLoaded()
        {
            loading = false;
        }

        protected void HideLoading()
        {
            loadingIcon.Run(new Sequence(
            [
                new FadeOut(0.3f),
                new Remove()
            ]));
        }

        protected virtual void GetData()
        {
            if (gamer == null)
            {
                if (Gamer.SignedInGamers.Count > 0)
                {
                    gamer = Gamer.SignedInGamers[PlayerIndex.One];
                    Label label = ContreJourLabel.CreateLabel(16f, gamer.DisplayName, true);
                    container.AddChild(label);
                    label.Position = new Vector2(pictureBackground.X, 285f);
                    label.Color = Color.Black;
                    GetGamerProfile(gamer);
                    return;
                }
                OnNotSignedError();
            }
        }

        private void GetGamerProfile(Gamer gamer)
        {
            gamer.GetProfile(new Action<GamerProfile>(OnGetGamerProfile), new Action(OnGetGamerProfileError));
        }

        private void OnGetGamerProfileError()
        {
            OnError(new Action<AsyncCallback>(MessageBoxes.ShowInternetError));
        }

        private void OnGetGamerProfile(GamerProfile result)
        {
            gamerProfile = result;
            Texture2D texture2D = Texture2D.FromStream(Mokus2DGame.Device, gamerProfile.GetGamerPicture());
            picture = new Sprite(texture2D)
            {
                Scale = 1.4f
            };
            pictureBackground.AddChild(picture);
            ShowItem(picture);
            ProcessGamerProfile();
        }

        protected void OnNotSignedError()
        {
            OnError(new Action<AsyncCallback>(MessageBoxes.ShowNotSignedToXBoxError));
        }

        private void OnError(Action<AsyncCallback> showMessageAction)
        {
            if (Open)
            {
                showMessageAction.Invoke(new AsyncCallback(Close));
            }
            SetNotLoaded();
        }

        protected void OnInternetError()
        {
            OnError(new Action<AsyncCallback>(MessageBoxes.ShowInternetError));
        }

        protected virtual void ProcessGamerProfile()
        {
        }

        protected const float PictureOffset = 50f;

        protected const float PictureSize = 64f;

        private const float ScrollOffset = 20f;

        private static Vector2 StartScrollPosition = new(231f, 402f);

        private readonly ScrollLayer scrollLayer = new();

        protected SignedInGamer gamer;

        private bool loading;

        protected GamerProfile gamerProfile;

        private float currentItemPosition;

        protected Label titleLabel;

        private readonly FakeHero hero = new();

        private readonly List<XBoxItem> items = new(64);

        private XBoxItem targetItem;

        private Sprite picture;

        private readonly Sprite pictureBackground;

        private Sprite loadingIcon;
    }
}

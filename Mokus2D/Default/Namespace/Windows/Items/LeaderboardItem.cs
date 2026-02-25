using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace.Windows.Items
{
    public class LeaderboardItem : XBoxItem
    {
        public LeaderboardItem(SignedInGamer gamer, LeaderboardEntry entry, int index)
            : base("McLeaderboardItemBackground")
        {
            Label label = ContreJourLabel.CreateLabel(18f, string.Format("{0}. {1}", index + 1, entry.Gamer.DisplayName), true);
            label.AnchorX = 0f;
            label.Position = new Vector2(83f, -37f);
            AddChild(label);
            scoreLabel = ContreJourLabel.CreateLabel(16f, entry.Rating.ToString(), true);
            scoreLabel.AnchorX = 0.5f;
            scoreLabel.Position = new Vector2(508f, -37f);
            AddChild(scoreLabel);
            if (gamer.Privileges.AllowProfileViewing != GamerPrivilegeSetting.Blocked)
            {
                entry.Gamer.GetProfile(new Action<GamerProfile>(OnGetProfile), new Action(MessageBoxes.ShowInternetError));
            }
        }

        public override Node ViewTarget => scoreLabel;

        public override Vector2 Size => new Vector2(400f, 100f);

        private void OnGetProfile(GamerProfile result)
        {
            Texture2D texture2D = Texture2D.FromStream(Mokus2DGame.Device, result.GetGamerPicture());
            Sprite sprite = new(texture2D)
            {
                OpacityFloat = 0f
            };
            sprite.Run(new FadeIn(0.5f));
            CreatePicture(sprite);
            sprite.Y -= 1.7f;
        }

        private const string Format = "{0}. {1}";

        private readonly Label scoreLabel;
    }
}

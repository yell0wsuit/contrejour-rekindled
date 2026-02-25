using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D;
using Mokus2D.Visual;

namespace Default.Namespace.Windows.Items
{
    public class AchievementItem : XBoxItem
    {
        public AchievementItem(Achievement achievement)
            : base("McAchievementItemBackground")
        {
            picture = GetPicture(achievement);
            CreatePicture(picture);
            Label label = ContreJourLabel.CreateLabel(16f, achievement.Name, false);
            label.AnchorX = 0f;
            label.Position = new Vector2(83f, -22f);
            AddChild(label);
            float num = (ContreJourLabel.CultureName == "de") ? 8.25f : 9f;
            Label label2 = ContreJourLabel.CreateLabel(num, achievement.Description, false);
            label2.Y = -10f;
            AddChild(label2);
            label2.AnchorX = 1f;
            label2.Position = new Vector2(494f, -54f);
            goldLabel = ContreJourLabel.CreateLabel(16f, achievement.GamerScore.ToString(), true);
            goldLabel.Position = new Vector2(532f, -37f);
            AddChild(goldLabel);
        }

        private static Sprite GetPicture(Achievement achievement)
        {
            Sprite sprite;
            if (achievement.IsEarned)
            {
                Texture2D texture2D = Texture2D.FromStream(Mokus2DGame.Device, achievement.GetPicture());
                sprite = new Sprite(texture2D);
            }
            else
            {
                sprite = new Sprite("McAchievementLock");
            }
            return sprite;
        }

        public override Node ViewTarget => goldLabel;

        public override Vector2 Size => ItemSize;

        private static readonly Vector2 ItemSize = new(500f, 84f);

        private readonly Label goldLabel;

        private readonly Sprite picture;
    }
}

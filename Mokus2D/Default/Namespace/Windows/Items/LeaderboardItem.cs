using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace.Windows.Items
{
    public class LeaderboardItem : XBoxItem
    {
        public LeaderboardItem(string name, long rating, int index)
            : base("McLeaderboardItemBackground")
        {
            Label label = ContreJourLabel.CreateLabel(18f, string.Format("{0}. {1}", index + 1, name), true);
            label.AnchorX = 0f;
            label.Position = new Vector2(83f, -37f);
            AddChild(label);
            scoreLabel = ContreJourLabel.CreateLabel(16f, rating.ToString(), true);
            scoreLabel.AnchorX = 0.5f;
            scoreLabel.Position = new Vector2(508f, -37f);
            AddChild(scoreLabel);
        }

        public override Node ViewTarget => scoreLabel;

        public override Vector2 Size => new(400f, 100f);

        private readonly Label scoreLabel;
    }
}

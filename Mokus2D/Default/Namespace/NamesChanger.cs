using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class NamesChanger : Node
    {
        public NamesChanger()
        {
            for (int i = 0; i < Constants.ChaptersCount; i++)
            {
                Node node = CreateChapterName(i);
                names.Add(node);
            }
            if (Constants.IsTrial)
            {
                Node node2;
                if (ContreJourLabel.IsEnglish)
                {
                    node2 = ClipFactory.CreateWithAnchor("McChapterMoreName");
                }
                else
                {
                    node2 = CreateLabelColor("GET_MORE_LEVELS", new Color(133, 213, 255));
                    node2.Scale = 0.8f;
                    foreach (Node node3 in node2.Children)
                    {
                        node3.X += 20f;
                    }
                }
                names.Add(node2);
            }
            foreach (Node node4 in names)
            {
                AddChild(node4);
                node4.Scale *= 0.9f;
                if (!Constants.IS_IPAD)
                {
                    node4.Scale *= 0.9375f;
                }
                node4.Visible = false;
            }
            names[1].Color = ContreJourConstants.WHITE_COLOR * 0.8f;
            if (Constants.IsTrial)
            {
                names[2].Color = names[1].Color;
            }
            currentIndex = -1f;
            CurrentIndex = 0f;
            screenSize = Mokus2DGame.ScreenSize;
        }

        public float CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (currentIndex != value)
                {
                    foreach (Node node in names)
                    {
                        node.Visible = false;
                    }
                    currentIndex = value;
                    float num = Maths.fmodf(currentIndex, names.Count);
                    if (num < 0f)
                    {
                        num += names.Count;
                    }
                    int num2 = (int)num % names.Count;
                    int num3 = (num2 + 1) % names.Count;
                    float num4 = Maths.fmodf(currentIndex, 1f);
                    if (num4 < 0f)
                    {
                        num4 += 1f;
                    }
                    Node node2 = names[(num3 + 1) % names.Count];
                    node2.Visible = false;
                    Node node3 = names[num2];
                    Node node4 = names[num3];
                    node3.Position = new Vector2(-num4 * 1300f, 0f);
                    node4.Position = new Vector2((1f - num4) * 1300f, 0f);
                    node3.Visible = true;
                    node4.Visible = Maths.FuzzyNotEquals(num4, 0f, 0.0001f);
                }
            }
        }

        private Node CreateChapterName(int index)
        {
            if (ContreJourLabel.IsEnglish)
            {
                return ClipFactory.CreateWithAnchor(string.Format("McChapter{0}Name", index + 1));
            }
            Color color = (index == 5) ? ContreJourConstants.GreenLightColor : ((index == 1) ? (ContreJourConstants.BLUE_LIGHT_COLOR * 1.8f) : ((index == 3) ? (ContreJourConstants.WHITE_LIGHT_COLOR * 1.8f) : Color.Black));
            return CreateLabelColor(string.Format("CHAPTER{0}", index + 1), color);
        }

        private Node CreateLabelColor(string text, Color color)
        {
            Node node = new();
            Label label = ContreJourLabel.CreateMultilineLabel(32f, text);
            label.Color = color;
            Label label2 = ContreJourLabel.CreateMultilineLabel(32f, text);
            label2.Color = Color.Black;
            label2.Opacity = 80;
            label2.Position = new Vector2(3f, -3f);
            label.AnchorY = label2.AnchorY = 0.5f;
            node.AddChild(label2);
            node.AddChild(label);
            return node;
        }

        private const float WIDTH = 1300f;

        private const int FontSize = 32;

        protected float currentIndex;

        protected List<Node> names = new();

        protected CGSize screenSize;
    }
}

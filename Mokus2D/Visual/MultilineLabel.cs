using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class MultilineLabel : Label
    {
        public MultilineLabel(SpriteFont font, string textString)
            : base(font, textString)
        {
        }

        public MultilineLabel(SpriteFont font)
            : base(font)
        {
        }

        public MultilineLabel()
        {
        }

        public override Vector2 Size => new Vector2(base.Size.X, base.Size.Y + LineSpacing * lines.Count);

        public float LineSpacing
        {
            get => lineSpacing;
            set
            {
                lineSpacing = value;
                textDirty = true;
            }
        }

        public void SetVerticalAnchorToLine(int lineNumber, float inLineAnchor = 0.5f)
        {
            TryRefreshText();
            AnchorY = (lineNumber + inLineAnchor) / lines.Count;
        }

        protected override void RefreshText()
        {
            base.RefreshText();
            lines.Clear();
            Split(text, '\n', lines);
            sizes.Clear();
            sizes.EnsureCapacity(lines.Count);
            foreach (StringBuilder stringBuilder in lines)
            {
                sizes.Add(MeasureString(stringBuilder).X);
            }
        }

        private static void Split(StringBuilder input, char separator, List<StringBuilder> result)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == separator)
                {
                    result.Add(stringBuilder);
                    stringBuilder = new StringBuilder();
                }
                else
                {
                    stringBuilder.Append(input[i]);
                }
            }
            if (stringBuilder.Length > 0)
            {
                result.Add(stringBuilder);
            }
        }

        protected override void DrawText(VisualState state, Color color)
        {
            Vector2 zero = Vector2.Zero;
            float num = (base.Size.Y / lines.Count + LineSpacing) * state.SpritesScaleFactor.Y;
            for (int i = 0; i < lines.Count; i++)
            {
                StringBuilder stringBuilder = lines[i];
                zero.X = (Size.X - sizes[i]) * Anchor.X;
                DrawLine(state, color, stringBuilder, zero);
                zero.Y += num;
            }
        }

        private const char SEPARATOR = '\n';

        private List<StringBuilder> lines = new(16);

        private List<float> sizes = new(16);

        private float lineSpacing;
    }
}

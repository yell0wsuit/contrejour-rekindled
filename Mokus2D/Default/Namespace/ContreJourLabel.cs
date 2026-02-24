using System.Collections.Generic;
using System.Globalization;

using ContreJour;

using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class ContreJourLabel : Label
    {
        public static Label CreateLabel(float size, bool applyScale = true)
        {
            return ProcessLabel(new Label(), size, applyScale);
        }

        public static T ProcessLabel<T>(T label, float size, bool applyScale = true) where T : Label
        {
            float num = 0.72f;
            if (applyScale)
            {
                if (IsSmallAsian)
                {
                    num *= 0.7f;
                }
                else if (NotEnglish)
                {
                    num *= 0.85f;
                }
            }
            SpriteFont font = GetFont(size * num, out float num2);
            label.Font = font;
            label.Scale = num2;
            return label;
        }

        public static Label CreateLabel(float size, string text, bool applyScale = true)
        {
            return ProcessLabel(new Label(null, text.Localize()), size, applyScale);
        }

        public static MultilineLabel CreateMultilineLabel(float size, string text)
        {
            MultilineLabel multilineLabel = ProcessLabel(new MultilineLabel(null, text.Localize()), size, true);
            multilineLabel.LineSpacing = -8f;
            multilineLabel.SetVerticalAnchorToLine(0, 0.5f);
            return multilineLabel;
        }

        public static ProgressLabel CreateProgressLabel(float size, string format, int value, int steps)
        {
            return ProcessLabel(new ProgressLabel(null, format, value, steps), size, true);
        }

        private static SpriteFont GetFont(float size, out float scale)
        {
            SpriteFont spriteFont = null;
            scale = 1.5f;
            foreach (KeyValuePair<int, SpriteFont> keyValuePair in ContreJourApplication.Fonts)
            {
                if (keyValuePair.Key >= size)
                {
                    spriteFont = keyValuePair.Value;
                    scale *= size / keyValuePair.Key;
                    break;
                }
            }
            return spriteFont;
        }

        private const float BASE_SCALE = 1.5f;

        private const float FontSizeMult = 0.72f;

        private static readonly List<string> SmallAsianLanguages = new(["ja", "ko"]);

        private static readonly List<string> Locales = new(["de", "es", "fr", "it", "nl", "ru", "uk", "zh"]);

        public static readonly string CultureName = CultureInfo.CurrentUICulture.Name[..2];

        private static readonly bool IsSmallAsian = SmallAsianLanguages.Contains(CultureName);

        public static readonly bool IsAsian = IsSmallAsian || CultureName == "zh";

        private static readonly bool NotEnglish = IsSmallAsian || Locales.Contains(CultureName);

        public static readonly bool IsEnglish = !NotEnglish;
    }
}

using System;
using System.Collections.Generic;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BackgroundChanger
    {
        public BackgroundChanger(List<Sprite> backgrounds)
        {
            this.backgrounds = backgrounds;
            RefreshOpacity();
        }

        public int FirstIndex { get; private set; }

        public int NextIndex { get; private set; }

        public float Offset { get; private set; }

        public float CurrentIndex
        {
            get; set
            {
                if (field != value)
                {
                    field = value;
                    RefreshOpacity();
                }
            }
        }

        private void RefreshOpacity()
        {
            FirstIndex = (int)Maths.fmodP(CurrentIndex, ContreJourConstants.PlanetsCount);
            Offset = Maths.PeriodicOffset(CurrentIndex - FirstIndex, ContreJourConstants.PlanetsCount);
            NextIndex = (FirstIndex + 1) % ContreJourConstants.PlanetsCount;
            SetBackgroundOpacity(FirstIndex, Offset);
            SetBackgroundOpacity(NextIndex, 1f - Offset);
            for (int i = 0; i < backgrounds.Count; i++)
            {
                backgrounds[i].Visible = Math.Abs(Maths.PeriodicOffset(i - CurrentIndex, ContreJourConstants.PlanetsCount)) < 1f;
            }
        }

        private void SetBackgroundOpacity(int index, float offset)
        {
            Sprite sprite = backgrounds[index];
            sprite.OpacityFloat = 1f - offset;
            sprite.Visible = sprite.Opacity > 0;
        }

        private readonly List<Sprite> backgrounds;
    }
}

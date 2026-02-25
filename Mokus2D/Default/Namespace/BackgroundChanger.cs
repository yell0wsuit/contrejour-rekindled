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

        public int FirstIndex => firstIndex;

        public int NextIndex => nextIndex;

        public float Offset => offset;

        public float CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (currentIndex != value)
                {
                    currentIndex = value;
                    RefreshOpacity();
                }
            }
        }

        private void RefreshOpacity()
        {
            firstIndex = (int)Maths.fmodP(currentIndex, ContreJourConstants.PlanetsCount);
            offset = Maths.PeriodicOffset(currentIndex - firstIndex, ContreJourConstants.PlanetsCount);
            nextIndex = (firstIndex + 1) % ContreJourConstants.PlanetsCount;
            SetBackgroundOpacity(firstIndex, offset);
            SetBackgroundOpacity(nextIndex, 1f - offset);
            for (int i = 0; i < backgrounds.Count; i++)
            {
                backgrounds[i].Visible = Math.Abs(Maths.PeriodicOffset(i - currentIndex, ContreJourConstants.PlanetsCount)) < 1f;
            }
        }

        private void SetBackgroundOpacity(int index, float offset)
        {
            Sprite sprite = backgrounds[index];
            sprite.OpacityFloat = 1f - offset;
            sprite.Visible = sprite.Opacity > 0;
        }

        private readonly List<Sprite> backgrounds;

        private float currentIndex;

        private int firstIndex;

        private int nextIndex;

        private float offset;
    }
}

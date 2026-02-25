using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Controls;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class PlanetsSpinner : Node, IDisposable
    {
        public float CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                pager.SetTargetPosition((int)currentIndex);
                pager.CurrentPosition = currentIndex;
            }
        }

        public bool Enabled
        {
            get => pager.Enabled; set => pager.Enabled = value;
        }

        public bool Exploding { get; private set; }

        public PlanetsSpinner(MainMenu menu)
        {
            data = UserData.Instance;
            CreatePlanets(menu);
            RefreshPosition();
        }

        public override void Update(float time)
        {
            pager.Update(time);
            currentIndex = pager.CurrentPosition;
            RefreshPosition();
        }

        private void CreatePlanets(MainMenu menu)
        {
            List<Type> list = new(
            [
                typeof(Chapter1),
                typeof(Chapter2),
                typeof(Chapter3),
                typeof(Chapter4),
                typeof(Chapter5),
                typeof(Chapter6)
            ]);
            int totalStars = data.TotalStars;
            for (int i = 0; i < list.Count; i++)
            {
                bool flag = CocosUtil.lite(false, i < Constants.NormalChaptersCount && UserData.StarsToUnlock(i) > totalStars);
                Type type = flag ? typeof(ChapterLocked) : list[i];
                ChapterItem chapterItem = (ChapterItem)ReflectUtil.CreateInstance(type, [i, menu]);
                if (!flag && i >= data.UnlockedChapters && i < Constants.NormalChaptersCount && UserData.StarsToUnlock(i) > 0)
                {
                    CreateExplodingChapter(chapterItem, menu);
                }
                AddChild(chapterItem);
                chapters.Add(chapterItem);
                chapterItem.SelectEvent += new Action<int>(OnSelect);
            }
        }

        private void OnSelect(int index)
        {
            SelectEvent.SendEvent(index);
            pager.SetTargetPosition((int)Math.Round((double)pager.CurrentPosition));
        }

        private void CreateExplodingChapter(ChapterItem chapter, MainMenu menu)
        {
            Exploding = true;
            pager.Enabled = false;
            data.UnlockChapter(chapter.Index);
            explodingChapter = new ChapterLocked(chapter.Index, menu)
            {
                TargetChapter = chapter
            };
            chapter.AddChild(explodingChapter);
            explodingChapter.ExplodeEvent.AddListenerSelector(delegate
            {
                OnLockExplode(explodingChapter);
            });
            explodingChapter.IgnoreParentOpacity = true;
            chapter.Opacity = 0;
            SetTargetChapter(chapter.Index);
        }

        public void SetTargetChapter(int index)
        {
            pager.CurrentPosition = index - 1;
            _ = Schedule(delegate
            {
                pager.SetTargetPosition(index);
            }, 0.3f);
        }

        private void OnLockExplode(ChapterLocked chapter)
        {
            pager.Enabled = true;
            chapter.RemoveListeners();
        }

        private void RefreshPosition()
        {
            for (int i = 0; i < chapters.Count; i++)
            {
                ChapterItem chapterItem = chapters[i];
                float num = Maths.PeriodicOffset(i - currentIndex, ContreJourConstants.PlanetsCount);
                chapterItem.Visible = Math.Abs(num) < 1.5f;
                if (chapterItem.Visible)
                {
                    float num2 = (float)Math.Cos((double)num);
                    chapterItem.Depth = num2;
                    chapterItem.Scale = num2 * 1.5f;
                    float num3 = chapterItem.Scale * chapterItem.Scale;
                    chapterItem.X = (num * 550f) + (AccelerometerOffset.X * 200f * num3);
                    chapterItem.Y = AccelerometerOffset.Y * num3 / 4f;
                }
            }
        }

        public new void Dispose()
        {
            pager.Dispose();
            foreach (ChapterItem chapterItem in chapters)
            {
                chapterItem.RemoveListeners();
            }
        }

        private const float PlanetsDistance = 550f;

        public readonly EventSender<int> SelectEvent = new();

        private readonly List<ChapterItem> chapters = new();

        private float currentIndex;

        private readonly UserData data;

        private ChapterLocked explodingChapter;

        private readonly GesturePager pager = new();

        public Vector2 AccelerometerOffset = Vector2.Zero;
    }
}

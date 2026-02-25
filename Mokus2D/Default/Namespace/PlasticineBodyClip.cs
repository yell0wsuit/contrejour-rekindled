using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class PlasticineBodyClip : ContreJourBodyClip, IRestartable
    {
        public PlasticineBodyClip(LevelBuilderBase _builder, List<Vector2> points, Node _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            ContreJourGame contreJourGame = (ContreJourGame)_builder.Game;
            contreJourGame.RegisterPlasticine(this);
            Type type = contreJourGame.ChooseSide(typeof(BlackPlasticineSprite), typeof(WhitePlasticineSprite), typeof(PlasticineSprite));
            clipContent = (PlasticineSprite)ReflectUtil.CreateInstance(type, []);
            Create(points);
            _ = builder.AddChild(clipContent);
            firstItem.BodyClip.UpdateParent = true;
            wideBorder = new PlasticineWideBorder();
            _ = builder.AddChild(wideBorder);
            InitBorder(contreJourGame);
            InitFillSprite();
            if (CocosUtil.isArmV7() && !contreJourGame.RoseChapter)
            {
                highlite = new PlasticineHighliteBorder(firstItem, wideBorder);
            }
            if (highlite != null)
            {
                _ = builder.AddChild(highlite);
            }
            changed = false;
            draggingItems = new Dictionary<Touch, DraggingItem>();
        }

        public PlasticineItem FirstItem => firstItem;

        public bool Changed
        {
            get => changed; set => changed = value;
        }

        public void Restart()
        {
            PlasticineItem nextItem = firstItem;
            do
            {
                nextItem.BodyClip.MoveToInitialPosition();
                nextItem = nextItem.NextItem;
            }
            while (nextItem != firstItem);
        }

        private void InitFillSprite()
        {
            int num = 0;
            int num2 = 0;
            PlasticineItem plasticineItem = leftItem.NextItem;
            PlasticineItem plasticineItem2 = leftItem.PreviousItem;
            do
            {
                if (num % 2 == 0)
                {
                    InitFillSpriteVertices(plasticineItem, plasticineItem2, ref num2);
                }
                plasticineItem = plasticineItem.NextItem;
                plasticineItem2 = plasticineItem2.PreviousItem;
                num++;
            }
            while (plasticineItem != plasticineItem2 && plasticineItem.NextItem != plasticineItem2);
            if (plasticineItem == plasticineItem2)
            {
                plasticineItem.BodyClip.SetFillSprite(clipContent, num2++);
            }
            else
            {
                InitFillSpriteVertices(plasticineItem, plasticineItem2, ref num2);
            }
            clipContent.InitVertices(num2);
        }

        private void InitFillSpriteVertices(PlasticineItem top, PlasticineItem bottom, ref int vertices)
        {
            top.BodyClip.SetFillSprite(clipContent, vertices);
            bottom.BodyClip.SetFillSprite(clipContent, vertices + 1);
            vertices += 2;
        }

        private void InitBorder(ContreJourGame game)
        {
            PlasticineItem nextItem = firstItem;
            int num = 0;
            do
            {
                nextItem.BodyClip.SetWideBorder(wideBorder, num);
                num++;
                nextItem = nextItem.NextItem;
            }
            while (nextItem != firstItem);
            Color color;
            if (game.BlackSide)
            {
                color = PlasticineConstants.BLACK_BORDER_OUT_COLOR;
            }
            else if (game.WhiteSide)
            {
                color = PlasticineConstants.WHITE_GROUND_OUT_COLOR;
            }
            else
            {
                color = new Color(0, 0, 0, 0);
            }
            wideBorder.SetSizeBorderColorBorderOutColor(num, clipContent.Color, color);
        }

        public PlasticineItem GetClosestItem(Vector2 point)
        {
            PlasticineItem nextItem = firstItem;
            PlasticineItem plasticineItem = null;
            float num = 0f;
            do
            {
                float num2 = (nextItem.Body.Position - point).LengthSquared();
                if (num2 < num || plasticineItem == null)
                {
                    plasticineItem = nextItem;
                    num = num2;
                }
                nextItem = nextItem.NextItem;
            }
            while (nextItem != firstItem);
            return plasticineItem;
        }

        public bool PointInside(Vector2 point)
        {
            float num = 30f;
            List<Vector2> list = new();
            list.Add(new Vector2(point.X + num, point.Y));
            list.Add(new Vector2(point.X, point.Y + num));
            list.Add(new Vector2(point.X - num, point.Y));
            list.Add(new Vector2(point.X, point.Y - num));
            for (int i = 0; i < list.Count; i++)
            {
                Vector2 vector = list[i];
                List<Fixture> list2 = FarseerUtil.RaycastWorldStartPointEndPoint(builder.World, point, vector);
                bool flag = false;
                for (int j = 0; j < list2.Count; j++)
                {
                    BodyClip bodyClip = list2[j].Body.UserData as BodyClip;
                    if (bodyClip is not null and PlasticinePartBodyClip)
                    {
                        PlasticinePartBodyClip plasticinePartBodyClip = (PlasticinePartBodyClip)bodyClip;
                        if (plasticinePartBodyClip.Parent == this)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    return false;
                }
            }
            return true;
        }

        public void GetBorderVerticesOffset(ref List<Vector2> polygon, float offset)
        {
            PlasticineItem nextItem = firstItem;
            do
            {
                polygon.Add(CocosUtil.ccp2Point(nextItem.GetBorder(offset)));
                nextItem = nextItem.NextItem;
            }
            while (nextItem != firstItem);
        }

        public PlasticineBorder CreateOutBorder(float offset)
        {
            List<Vector2> list = new();
            GetBorderVerticesOffset(ref list, offset);
            return Game.BlackSide
                ? new BlackPlasticineBorder(list)
                : !Game.BonusChapter ? new PlasticineBorder(list) : new GreenPlasticineBorder(list);
        }

        public bool StartDragItemTouch(PlasticineItem item, Touch touch)
        {
            int i = 0;
            PlasticineItem plasticineItem = item.PreviousItem;
            PlasticineItem plasticineItem2 = item.NextItem;
            while (i < 7)
            {
                if (plasticineItem.BodyClip.Dragging || plasticineItem2.BodyClip.Dragging)
                {
                    return false;
                }
                plasticineItem = plasticineItem.PreviousItem;
                plasticineItem2 = plasticineItem2.NextItem;
                i++;
            }
            item.BodyClip.ScareFlyes(0);
            item.NextItem.BodyClip.ScareFlyes(0);
            item.NextItem.NextItem.BodyClip.ScareFlyes(0);
            item.PreviousItem.BodyClip.ScareFlyes(0);
            item.PreviousItem.PreviousItem.BodyClip.ScareFlyes(0);
            DraggingItem draggingItem = new(builder, item, touch);
            draggingItems[touch] = draggingItem;
            return true;
        }

        public void StopDragTouch(PlasticineItem item, Touch touch)
        {
            DraggingItem draggingItem = draggingItems[touch];
            _ = draggingItems.Remove(touch);
            draggingItem.Finish();
        }

        public void UpdateGraphics(float time)
        {
            if (builder.Game.Debug)
            {
                return;
            }
            lastTouchTime += time;
            PlasticineItem nextItem = firstItem;
            do
            {
                nextItem.BodyClip.UpdateWideBorderAndFill();
                nextItem = nextItem.NextItem;
            }
            while (nextItem != firstItem);
            if (highlite != null)
            {
                highlite.Update(time);
            }
            if (changed)
            {
                changed = false;
            }
        }

        public bool TouchMove(Touch touch)
        {
            DraggingItem draggingItem = draggingItems[touch];
            changed |= draggingItem.Update();
            lastTouchTime = 0f;
            return true;
        }

        public void SetDotPositionPosition(int index, Vector2 position)
        {
        }

        public void Create(List<Vector2> points)
        {
            firstItem = SurfaceCreator.CreateParentPointsMaxWidth((ContreJourLevelBuilder)builder, this, points, 0.6f, out leftItem);
        }

        private const float OPACITY_STEP = 5.1f;

        private const float END_OPACITY = 102f;

        private const int START_OPACITY = 0;

        private const float MAX_DRAG_FORCE = 800f;

        private const int GROUND_FILL_STEP = 2;

        protected bool changed;

        protected PlasticineSprite clipContent;

        protected Dictionary<Touch, DraggingItem> draggingItems;

        protected PlasticineItem firstItem;

        protected PlasticineHighliteBorder highlite;

        protected float lastTouchTime;

        private PlasticineItem leftItem;

        protected PlasticineWideBorder wideBorder;
    }
}

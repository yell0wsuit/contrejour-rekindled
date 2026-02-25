using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class PlanetLiana : Node, ILianaDrawData
    {
        public bool Stoped
        {
            get => stoped; set => stoped = value;
        }

        public LianaSprite Sprite => sprite;

        public PlanetLiana(Vector2 start, Vector2 _middle, Vector2 end)
        {
            Box2DConfig defaultConfig = Box2DConfig.DefaultConfig;
            points.Add(defaultConfig.ToVec(start));
            middle = defaultConfig.ToVec(_middle);
            points.Add(middle);
            points.Add(defaultConfig.ToVec(end));
            sprite = new LianaSprite(this, new Color(50, 50, 50, 255), Maths.randRange(2f, 4f), 2f);
            AddChild(sprite);
            angle = Maths.randRange(-0.5235988f, 0.5235988f);
            changer = new CosChanger(-Maths.randRange(0.1f, 0.5f), Maths.randRange(0.1f, 0.5f), Maths.randRange(0.01f, 0.02f));
        }

        public void ReduceRange()
        {
            changer.MinValue /= 2f;
            changer.MaxValue /= 2f;
        }

        public override void Update(float time)
        {
            if (!stoped)
            {
                changer.Update(time);
                points[1] = middle + FarseerUtil.toVec(changer.Value, angle);
            }
            sprite.Update(time);
        }

        public Vector2 PositionAt(int index)
        {
            return points[index];
        }

        public int PointsCount()
        {
            return points.Count;
        }

        protected List<Vector2> points = new();

        protected LianaSprite sprite;

        protected CosChanger changer;

        protected Vector2 middle;

        protected float angle;

        protected bool stoped;
    }
}

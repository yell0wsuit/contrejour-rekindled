using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class Chapter5(int _index, MainMenu _menu) : ChapterItem(_index, _menu)
    {
        protected override void CreateSprites()
        {
            float num = 2.5f;
            background = ClipFactory.CreateWithAnchor("McPlanet5Background");
            container.AddChild(background);
            background.Scale = num;
            ocean = ClipFactory.CreateWithAnchor("McPlanet5Ocean");
            container.AddChild(ocean);
            ocean.Scale = num;
            CreateLianas();
            planetForeground = ClipFactory.CreateWithAnchor("McPlanet5Foreground");
            container.AddChild(planetForeground);
            blurBackground = ClipFactory.CreateWithAnchor("McPlanet5Blur");
            for (int i = 1; i <= 4; i++)
            {
                string text = string.Format("McHole{0}", i);
                Node node = ClipFactory.CreateWithAnchor(text);
                container.AddChild(node);
                CosOpacityChanger cosOpacityChanger = new(node, 0f, 1f, Maths.randRange(0.02f, 0.07f) / 255f);
                changers.Add(cosOpacityChanger);
            }
            ParticleSystem particleSystem = new("McEnergyBall.png");
            container.AddChild(particleSystem);
            PlanetEnergy planetEnergy = new(particleSystem, CocosUtil.ccpIPad(35f, 120f), null);
            AddUpdating(planetEnergy);
            particleSystem.Scale = 0.55f;
            alphaItems.Add(particleSystem);
            CreateForegrounds();
        }

        public void CreateLianas()
        {
            AddLianaMiddleEnd(new Vector2(-25f, -103f), new Vector2(15f, -139f), new Vector2(57f, -64f));
            AddLianaMiddleEndReduce(new Vector2(-116f, 37f), new Vector2(-114f, 10f), new Vector2(-72f, 0f), true);
            AddLianaMiddleEndReduce(new Vector2(-109f, 27f), new Vector2(-103f, -7f), new Vector2(-79f, -21f), true);
            AddLianaMiddleEnd(new Vector2(26f, -99f), new Vector2(60f, -111f), new Vector2(77f, -29f));
            AddLianaMiddleEndReduce(new Vector2(-90f, 45f), new Vector2(-64f, 78f), new Vector2(-43f, 66f), true);
            AddLianaMiddleEnd(new Vector2(-37f, -123f), new Vector2(-6f, -165f), new Vector2(30f, -92f));
        }

        public void CreateForegrounds()
        {
            foreground = new Node();
            foregroundContainer = new Node
            {
                Visible = false
            };
            foregroundContainer.AddChild(foreground);
            foreground.Position = -ScreenConstants.IPhoneScreenCenter;
            foregroundContainer.Position = -foreground.Position;
            menu.AddForeground(foregroundContainer);
            _ = AddForegroundPositionScaleAngle("McLeafView4", CocosUtil.iPad(new Vector2(624f, 721f), CocosUtil.ccpIPad(624f, 601f)), new Vector2(1.72f, 1.29f), 171f);
            _ = AddForegroundPositionScaleAngle("McLeafView3", CocosUtil.iPad(new Vector2(731f, 714f), CocosUtil.ccpIPad(731f, 584f)), new Vector2(2.37f, 2.37f), -172f);
            _ = AddForegroundPositionScaleAngle("McLeafView5", CocosUtil.iPad(new Vector2(978f, 693f), CocosUtil.ccpIPad(978f, 563f)), new Vector2(3.31f, 3.31f), -22f);
            _ = AddForegroundPositionScaleAngleRotationOffset("McLeafView1", CocosUtil.iPad(new Vector2(1095f, 644f), CocosUtil.ccpIPad(1095f, 564f)), new Vector2(2.71f, 2.71f), 0f, 5f);
            _ = AddForegroundPositionScaleAngleRotationOffset("McLeafView0", CocosUtil.iPad(new Vector2(1113f, 575f), CocosUtil.ccpIPad(1113f, 475f)), new Vector2(2.38f, 2.38f), -30f, -3f);
            _ = AddForegroundPositionScaleAngleRotationOffset("McLeafView0", new Vector2(1072f, 108f), new Vector2(2.45f, 2.45f), -52f, 1f);
            _ = AddForegroundPositionScaleAngleRotationOffset("McLeafView2", new Vector2(1092f, 167f), new Vector2(2.17f, 2.17f), -14f, -3f);
            _ = AddForegroundPositionScaleAngle("McLeafView4", new Vector2(409f, 66f), new Vector2(-1.7f, 1.4f), 0f);
            _ = AddForegroundPositionScaleAngle("McLeafView3", new Vector2(340f, 60f), new Vector2(-2.68f, 2.68f), 4f);
            _ = AddForegroundPositionScaleAngle("McLeafView5", new Vector2(262f, 63f), new Vector2(-2.13f, 2.13f), 0f);
            _ = AddForegroundPositionScaleAngleRotationOffset("McLeafView2", CocosUtil.ccpIPad(CocosUtil.iPad(-64, -124), 366f), new Vector2(-2.97f, 2.97f), 0f, 4f);
            _ = AddForegroundPositionScaleAngleRotationOffset("McLeafView1", CocosUtil.ccpIPad(CocosUtil.iPad(-112, -182), 404f), new Vector2(-2.76f, 2.76f), -12f, -3f);
        }

        public void AddLianaMiddleEndReduce(Vector2 start, Vector2 middle, Vector2 end, bool reduce)
        {
            PlanetLiana planetLiana = new(start, middle, end);
            container.AddChild(planetLiana);
            AddUpdating(planetLiana);
            if (reduce)
            {
                planetLiana.ReduceRange();
            }
            AddAlphaItem(planetLiana.Sprite);
        }

        public void AddLianaMiddleEnd(Vector2 start, Vector2 middle, Vector2 end)
        {
            AddLianaMiddleEndReduce(start, middle, end, false);
        }

        protected override void RefreshDepth()
        {
            base.RefreshDepth();
            foreground.OpacityFloat = Maths.max((depth - 0.8f) * 5f, 0f);
            foregroundContainer.Visible = foreground.Opacity > 0;
            if (foregroundContainer.Visible)
            {
                foregroundContainer.Scale = 10f - ((depth - 0.8f) * 5f * 9f);
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            foreach (CosPropertyChanger cosPropertyChanger in changers)
            {
                cosPropertyChanger.Update(time);
            }
            ocean.Rotation += time * 11f;
            background.Rotation -= time * 7f;
        }

        private Node AddForegroundPositionScaleAngleRotationOffset(string name, Vector2 position, Vector2 scale, float angle, float _offset)
        {
            Node node = AddForegroundPositionScaleAngle(name, position, scale, angle);
            CosRotationChanger cosRotationChanger = new(node, _offset, Maths.randRange(0.005f, 0.01f));
            changers.Add(cosRotationChanger);
            return node;
        }

        public Node AddForegroundPositionScaleAngle(string name, Vector2 position, Vector2 scale, float angle)
        {
            Node node = ClipFactory.CreateWithAnchor(name);
            node.Position = position;
            node.ScaleX = scale.X;
            node.ScaleY = scale.Y;
            node.Rotation = angle;
            foreground.AddChild(node);
            return node;
        }

        protected Node foreground;

        protected Node foregroundContainer;

        protected List<CosPropertyChanger> changers = new();

        protected Node planetForeground;

        protected Sprite ocean;
    }
}

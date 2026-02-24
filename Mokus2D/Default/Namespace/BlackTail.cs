using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class BlackTail : PrimitivesNode, IUpdatable
    {
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Frames
        {
            get
            {
                return frames;
            }
            set
            {
                frames = value;
            }
        }

        public Body Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }

        public Vector2 Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public override float OpacityFloat
        {
            set
            {
                if (value != OpacityFloat)
                {
                    base.OpacityFloat = value;
                    opacityDirty = true;
                }
            }
        }

        public BlackTail(Body _body, ContreJourLevelBuilder _builder)
            : this(_body, _builder, _builder.ContreJour.BonusChapter ? "McTailTextureGreen.png" : "McTailTexture.png")
        {
        }

        public BlackTail(Body _body, LevelBuilderBase _builder, string textureFile)
        {
            UpdateEnabled = false;
            body = _body;
            builder = _builder;
            frames = 40;
            previousPosition = CocosUtil.ccp2Point(_builder.ToPoint(body.Position));
            currentFrame = 0;
            width = CocosUtil.r(40f);
            Texture = ClipFactory.CreateWithoutConfig(textureFile);
        }

        public BlackTail(BodyClip _clip, string textureFile)
            : this(_clip.Body, _clip.Builder, textureFile)
        {
        }

        public BlackTail(ContreJourBodyClip _clip)
            : this(_clip.Body, (ContreJourLevelBuilder)_clip.Builder)
        {
        }

        public bool Moving
        {
            get
            {
                return moving;
            }
            set
            {
                if (moving != value)
                {
                    moving = value;
                }
            }
        }

        public int Length
        {
            get
            {
                return bezierPoints.Count;
            }
        }

        public override void Update(float time)
        {
            currentPosition = CocosUtil.ccp2Point(builder.ToPoint((body != null) ? body.Position : target));
            bool flag = true;
            if (!Maths.ccpEqual(previousPosition, Vector2.Zero))
            {
                currentCenter = currentPosition.Middle(previousPosition);
                if (!Maths.ccpEqual(previousCenter, Vector2.Zero))
                {
                    float num = currentCenter.DistanceTo(previousCenter);
                    flag = num > 1f;
                    if (flag)
                    {
                        int num2 = (int)Math.Ceiling((double)(num / 3f));
                        List<Vector2> list = new();
                        Maths.GetBezierPointsControlDestinationSegmentsInsertLastResult(previousCenter, previousPosition, currentCenter, num2, false, list);
                        for (int i = 0; i < list.Count; i++)
                        {
                            removeFrames.Insert(0, currentFrame);
                        }
                        if (bezierPoints.Count == 0)
                        {
                            bezierPoints.Add(Vector2.Zero);
                        }
                        bezierPoints[0] = currentPosition;
                        bezierPoints.InsertRange(1, new ReverseDecorator<Vector2>(list));
                    }
                }
                else
                {
                    removeFrames.Insert(0, currentFrame);
                    bezierPoints.Insert(0, previousPosition);
                }
                List<Pair<Vector2>> list2 = CreatePairs();
                Array.Resize(ref vertices, list2.Count * 2);
                if (list2.Count > 1)
                {
                    for (int j = 0; j < list2.Count; j++)
                    {
                        int num3 = j * 2;
                        vertices[num3].Position = list2[j].First.ToVector3();
                        vertices[num3].TextureCoordinate = new Vector2(j, 0f);
                        vertices[num3 + 1].Position = list2[j].Second.ToVector3();
                        vertices[num3 + 1].TextureCoordinate = new Vector2(j, 1f);
                        vertices[num3].Color = GetTailColor();
                        vertices[num3 + 1].Color = GetTailColor();
                    }
                }
            }
            if (flag)
            {
                previousCenter = currentCenter;
                previousPosition = currentPosition;
            }
            RemoveTail(time);
            currentFrame++;
        }

        public int FramesToLive(float time)
        {
            return frames;
        }

        public void RemoveTail(float time)
        {
            int num = currentFrame - FramesToLive(time);
            while (removeFrames.Count > 0 && removeFrames[removeFrames.Count - 1] <= num)
            {
                removeFrames.RemoveAt(removeFrames.Count - 1);
                bezierPoints.RemoveAt(bezierPoints.Count - 1);
            }
        }

        public List<Pair<Vector2>> CreatePairs()
        {
            List<Pair<Vector2>> list = new();
            int num = bezierPoints.Count + 1;
            for (int i = bezierPoints.Count - 1; i >= 0; i--)
            {
                Vector2 vector = ((i == bezierPoints.Count - 1) ? bezierPoints[bezierPoints.Count - 1] : bezierPoints[i + 1]);
                Vector2 vector2 = ((i == 0) ? currentPosition : bezierPoints[i - 1]);
                float num2 = width * (1f - (i + 1) / (float)num);
                if (num2 > 1f)
                {
                    Pair<Vector2> pointsPair = ContreDrawUtil.GetPointsPair(bezierPoints[i], vector, vector2, num2);
                    list.Add(pointsPair);
                }
            }
            return list;
        }

        protected override void DrawPrimitives()
        {
            if (vertices.Length > 3)
            {
                if (opacityDirty)
                {
                    GraphUtil.SetColor(vertices, GetTailColor());
                }
                GraphUtil.FillTrianglesStrip(vertices, Texture);
            }
        }

        private Color GetTailColor()
        {
            return new Color(OpacityFloat, OpacityFloat, OpacityFloat, OpacityFloat);
        }

        private const int LOW_FPS_FRAMES_TO_LIVE = 10;

        private const int FRAMES_TO_LIVE = 40;

        private const float MIN_DISTANCE = 1f;

        private const float PART_DISTANCE = 3f;

        private const float TAIL_WIDTH = 40f;

        protected VertexPositionColorTexture[] vertices = [];

        protected List<Vector2> bezierPoints = new();

        protected Body body;

        protected Vector2 target;

        protected LevelBuilderBase builder;

        protected Vector2 currentPosition;

        protected Vector2 previousPosition;

        protected Vector2 previousCenter;

        protected Vector2 currentCenter;

        protected List<int> removeFrames = new();

        protected int currentFrame;

        protected float width;

        protected int frames;

        protected bool moving;

        private float LOW_FPS_TIME = 0.04f;

        private bool opacityDirty;
    }
}

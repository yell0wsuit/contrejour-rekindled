using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Util;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class ChapterItem : Node
    {
        public ChapterItem(int _index, MainMenu _menu)
        {
            menu = _menu;
            index = _index;
            depth = -1f;
            container = new Node();
            if (!HardwareCapabilities.IsLowMemoryDevice)
            {
                CreateBackLight();
            }
            AddChild(container);
            CreateSprites();
            AddChild(blurBackground);
            hidingItems.Add(background);
            offset = index * 3.1415927f * 2f / ContreJourConstants.PlanetsCount;
            CreateClickListener();
            enabled = true;
        }

        public float Offset
        {
            get
            {
                return offset;
            }
        }

        public virtual float Depth
        {
            get
            {
                return depth;
            }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    RefreshDepth();
                }
            }
        }

        public Color LightColor
        {
            get
            {
                return lightColor;
            }
            set
            {
                lightColor = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public override float OpacityFloat
        {
            set
            {
                base.OpacityFloat = value;
                RefreshDepth();
            }
        }

        public override void Update(float time)
        {
            if (depth > 0.75f)
            {
                float num = Math.Max((depth - 0.75f) * 4f * time, 0f);
                foreach (IUpdatable updatable in updating)
                {
                    updatable.Update(num);
                }
            }
        }

        public event Action<int> SelectEvent;

        protected virtual void CreateClickListener()
        {
            clickListener = new RadiusClickListener(this, CocosUtil.iPad(180, 90), 0);
            clickListener.Radius = 40f;
            clickListener.DisableDrag = true;
            clickListener.ClickEvent.AddListenerSelector(new Action(OnClick));
        }

        protected void AddUpdating(IUpdatable item)
        {
            updating.Add(item);
            if (item is NodeBase)
            {
                ((NodeBase)item).UpdateEnabled = false;
            }
        }

        protected virtual void CreateBackLight()
        {
            backLight = ClipFactory.CreateWithAnchor("McChapterLight");
            backLight.Scale = 2.5f;
            AddChild(backLight);
            AddAlphaItem(backLight);
        }

        public void AddHidingItem(Node item)
        {
            hidingItems.Add(item);
        }

        public void AddAlphaItem(Node item)
        {
            alphaItems.Add(item);
        }

        protected virtual void CreateSprites()
        {
            background = ClipFactory.CreateWithAnchor("McPlanet1Background");
            blurBackground = ClipFactory.CreateWithAnchor("McChapter1Blur");
            AddChild(background);
        }

        protected virtual void RefreshDepth()
        {
            if (backLight != null)
            {
                backLight.Color = lightColor;
            }
            Color color = ColorUtil.Mult(lightColor, (1f - depth) * 0.3f);
            blurBackground.Color = color;
            float num = Maths.Clamp((depth - 0.7f) * Opacity / 0.3f, 0f, 255f);
            foreach (object obj in alphaItems)
            {
                Node node = (Node)obj;
                node.Opacity = (int)num;
                node.Visible = num > 0f;
            }
            float num2 = Maths.Clamp((1f - depth) / 0.2f * Opacity, 0f, 255f);
            blurBackground.Opacity = (int)num2;
            blurBackground.Visible = num2 > 0f;
            background.Opacity = (int)Maths.Clamp(num * 3f, 0f, 255f);
            container.Visible = num > 0f;
            foreach (object obj2 in depthDependent)
            {
                IDepthDependent depthDependent = (IDepthDependent)obj2;
                depthDependent.Depth = depth;
            }
        }

        public virtual void RemoveListeners()
        {
            clickListener.Enabled = false;
            clickListener.ClickEvent.RemoveListenerSelector(new Action(OnClick));
            clickListener.Remove();
        }

        protected virtual void OnClick()
        {
            if (depth > 0.99f)
            {
                OnSelect();
            }
        }

        public void OnSelect()
        {
            SelectEvent.Dispatch(index);
        }

        ~ChapterItem()
        {
        }

        private readonly List<IUpdatable> updating = new();

        protected ArrayList alphaItems = new();

        protected Sprite backLight;

        protected Sprite background;

        protected Sprite blurBackground;

        protected RadiusClickListener clickListener;

        protected Node container;

        protected float depth;

        protected ArrayList depthDependent = new();

        protected bool enabled;

        protected ArrayList hidingItems = new();

        protected int index;

        protected Color lightColor;

        protected MainMenu menu;

        protected float offset;
    }
}

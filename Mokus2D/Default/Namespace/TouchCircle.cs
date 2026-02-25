using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D.Input;

namespace Default.Namespace
{
    public class TouchCircle(Touch _touch, LevelBuilderBase _builder) : BodyClip(_builder, CreateBody(_builder, _touch), null, null)
    {
        public Touch Touch => touch;

        public bool Enabled
        {
            get => enabled; set => enabled = value;
        }

        public bool Free => free;

        public new Vector2 Position => builder.TouchRootPoint(touch);

        public static Body CreateBody(LevelBuilderBase _builder, Touch _touch)
        {
            Body body = FarseerUtil.CreateCircle(_builder.World, 4f, _builder.TouchRootVec(_touch), 0f, 0f, true);
            FarseerUtil.SetSensor(body, true);
            return body;
        }

        public override void Update(float time)
        {
            base.Update(time);
            Body.SetTransform(builder.TouchRootVec(touch), 0f);
            if (enabled)
            {
                ProcessContacts();
            }
        }

        public void ProcessContacts()
        {
            ContactEdge contactEdge = Body.ContactList;
            bool flag = false;
            while (contactEdge != null)
            {
                if (contactEdge.Contact.IsTouching)
                {
                    PlasticinePartBodyClip plasticinePartBodyClip = contactEdge.Other.UserData as PlasticinePartBodyClip;
                    if (plasticinePartBodyClip != null)
                    {
                        flag = true;
                    }
                }
                contactEdge = contactEdge.Next;
            }
            free = !flag;
        }

        public bool IsNegative(PlasticinePartBodyClip part)
        {
            if (!closestMap.ContainsKey(part.Parent))
            {
                closestMap[part.Parent] = new ClosestItem(null, false);
            }
            ClosestItem closestItem = closestMap[part.Parent];
            if (!closestItem.Refreshed)
            {
                RefreshClosestPlasticineDefaultItem(closestItem, part);
            }
            return closestItem.Negative;
        }

        public void RefreshClosestPlasticineDefaultItem(ClosestItem item, PlasticinePartBodyClip defaultItem)
        {
            if (item.Clip == null)
            {
                item.Clip = defaultItem;
            }
            float num = FarseerUtil.b2Vec2Distance(Body.Position, item.Clip.Body.Position);
            float num2 = FarseerUtil.b2Vec2Distance(Body.Position, defaultItem.Body.Position);
            if (num > num2)
            {
                num = num2;
                item.Clip = defaultItem;
            }
            PlasticineItem plasticineItem = item.Clip.Item;
            bool flag = false;
            bool flag2;
            do
            {
                flag2 = false;
                plasticineItem = plasticineItem.PreviousItem;
                float num3 = FarseerUtil.b2Vec2Distance(plasticineItem.Body.Position, Body.Position);
                if (num3 < num)
                {
                    flag2 = true;
                    flag = true;
                    num = num3;
                }
            }
            while (flag2);
            plasticineItem = plasticineItem.NextItem;
            if (!flag)
            {
                do
                {
                    flag2 = false;
                    plasticineItem = plasticineItem.NextItem;
                    float num4 = FarseerUtil.b2Vec2Distance(plasticineItem.Body.Position, Body.Position);
                    if (num4 < num)
                    {
                        flag2 = true;
                        num = num4;
                    }
                }
                while (flag2);
                plasticineItem = plasticineItem.PreviousItem;
            }
            item.Clip = plasticineItem.BodyClip;
            float projectionTarget = FarseerUtil.GetProjectionTarget(Body.Position - item.Clip.Body.Position, item.Clip.Normal);
            item.Negative = projectionTarget > 0.7f;
            item.Refreshed = true;
        }

        private void Dealloc()
        {
            builder.World.RemoveBody(Body);
        }

        private const float NEGATIVE_MULT = -0.25f;

        private const float FORCE = 350f;

        private const float MAX_OFFSET = 2.3333333f;

        private const float ACTION_RADIUS = 3.3333333f;

        private const float RADIUS = 4f;

        protected Dictionary<PlasticineBodyClip, ClosestItem> closestMap;

        protected bool enabled = true;

        protected bool free = true;

        protected Touch touch = _touch;

        public class ClosestItem
        {
            public ClosestItem()
            {
                Clip = null;
                Refreshed = false;
            }

            public ClosestItem(PlasticinePartBodyClip clip, bool refreshed)
            {
                Clip = clip;
                Refreshed = refreshed;
            }

            public PlasticinePartBodyClip Clip;

            public bool Negative;

            public bool Refreshed;
        }
    }
}

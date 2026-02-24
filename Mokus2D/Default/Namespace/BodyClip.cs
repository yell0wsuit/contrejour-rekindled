using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BodyClip : Updatable
    {
        public Hashtable Config
        {
            get
            {
                return config;
            }
            set
            {
                config = value;
            }
        }

        public Node Clip
        {
            get
            {
                return clip;
            }
        }

        public float RotationOffset
        {
            get
            {
                return rotationOffset;
            }
            set
            {
                rotationOffset = value;
            }
        }

        public LevelBuilderBase Builder
        {
            get
            {
                return builder;
            }
        }

        public BodyClip(LevelBuilderBase builder, object body, Node clip, Hashtable config)
        {
            if (config == null)
            {
                config = new Hashtable();
            }
            rotationOffset = config.GetFloat("rotationOffset", 0f);
            rotationOffsetRadians = rotationOffset.ToRadians();
            this.config = config;
            this.clip = clip;
            Body = (Body)body;
            this.builder = builder;
        }

        protected float InitialBodyAngle
        {
            get
            {
                return MathHelper.ToRadians(-rotationOffset);
            }
        }

        public float BodyAngle
        {
            get
            {
                return InitialBodyAngle + body.Rotation;
            }
        }

        public DelayedAction Schedule(Action action, float delay)
        {
            return builder.Game.GameRoot.Schedule(action, delay);
        }

        public void Unschedule(DelayedAction action)
        {
            builder.Game.GameRoot.StopAction(action);
        }

        public List<string> TexturesToUnload()
        {
            return new List<string>();
        }

        public virtual Vector2 PositionVec
        {
            get
            {
                return body.Position;
            }
        }

        public Vector2 Position
        {
            get
            {
                return CocosUtil.toIPad(builder.ToPoint(body.Position));
            }
        }

        private Vector2 Scale()
        {
            return config.GetVector("scale", Vector2.One);
        }

        public virtual Body Body
        {
            get
            {
                return body;
            }
            protected set
            {
                if (body != null)
                {
                    body.UserData = null;
                }
                body = value;
                if (body != null)
                {
                    object userData = body.UserData;
                    body.UserData = this;
                }
            }
        }

        public override void Update(float time)
        {
            if (body != null && clip != null)
            {
                UpdatePosition();
                UpdateRotation();
            }
        }

        public virtual void UpdatePosition()
        {
            clip.Position = CocosUtil.toIPad(builder.ToPoint(body.Position));
        }

        public virtual void UpdateRotation()
        {
            clip.RotationRadians = body.Rotation - rotationOffsetRadians;
        }

        public void DestroyLater()
        {
            if (destroyLaterCalled)
            {
                return;
            }
            destroyLaterCalled = true;
            CallAfter(new Action(Destroy), 0.01f);
        }

        public virtual void Clear()
        {
        }

        public void Destroy()
        {
            if (clip != null && clip.Parent != null)
            {
                clip.Parent.RemoveChild(clip);
            }
            RemoveBody();
        }

        public void RemoveBody()
        {
            if (destroyed)
            {
                return;
            }
            destroyed = true;
            body.UserData = null;
            builder.World.RemoveBody(body);
        }

        public virtual void OnCollisionPoint(Body body2, Contact point)
        {
        }

        public virtual void OnCollisionStartPoint(Body body2, Contact point)
        {
        }

        public virtual void OnCollisionEndPoint(Body body2, Contact point)
        {
        }

        protected CallAfterData CallAfter(Action action, float delay)
        {
            return builder.Game.Updater.CallAfterSelectorDelay(action, delay);
        }

        protected CallAfterData CallAfter<T>(Action<T> action, float delay, T parameter)
        {
            return builder.Game.Updater.CallAfterSelectorDelayParameter(action, delay, parameter);
        }

        public virtual void PostSolvePointImpulse(Body body2, Contact point, ContactVelocityConstraint impulse)
        {
        }

        protected float rotationOffset;

        protected float rotationOffsetRadians;

        private Body body;

        protected Node clip;

        protected Hashtable config;

        protected LevelBuilderBase builder;

        protected bool destroyed;

        protected bool destroyLaterCalled;
    }
}

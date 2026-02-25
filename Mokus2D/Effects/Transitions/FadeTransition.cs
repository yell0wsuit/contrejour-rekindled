using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Mokus2D.Effects.Transitions
{
    public class FadeTransition(float hideSeconds, float showSeconds, Node toRemove, Func<Node> nodeFactory, int framesToWait = 0) : NodeAction
    {
        public event Action MiddleEvent;

        public LayerColor Layer { get; } = new(Color.Black)
        {
            OpacityFloat = 0f
        };

        public FadeTransition(float seconds, Node toRemove, Func<Node> nodeFactory, int framesToWait = 0)
            : this(seconds / 2f, seconds / 2f, toRemove, nodeFactory, framesToWait)
        {
        }

        internal override void Start(float time)
        {
            base.Start(time);
            currentAction = new FadeIn(hideSeconds);
            Target.AddChild(Layer, toRemove.Layer + 1);
            Layer.Run(currentAction);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (dispatch)
            {
                dispatch = false;
                MiddleEvent.Dispatch();
            }
            if (currentAction == null)
            {
                if (currentFrame >= framesToWait)
                {
                    currentAction = new FadeOut(showSeconds);
                    Layer.Run(currentAction);
                }
                currentFrame++;
                return;
            }
            if (currentAction.Finished)
            {
                if (fadingOut && replaced)
                {
                    finished = true;
                    Finish();
                    return;
                }
                if (fadingOut && !replaced)
                {
                    replaced = true;
                    CocosUtil.ReplaceNodeWithCleanup(toRemove, nodeFactory.Invoke());
                    toRemove = null;
                    currentAction = null;
                    currentFrame = 0;
                    dispatch = true;
                    return;
                }
                if (!fadingOut)
                {
                    fadingOut = true;
                }
            }
        }

        protected void Finish()
        {
            Layer.RemoveFromParent();
        }

        private NodeAction currentAction;
        private bool fadingOut;

        private bool replaced;
        private int currentFrame;

        private bool dispatch;
    }
}

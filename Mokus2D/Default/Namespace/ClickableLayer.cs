using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Input;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class ClickableLayer(int priority = 0) : Node, ITouchListener
    {
        public bool Enabled { get; set; } = true;

        private bool CanProcessTouch
        {
            get
            {
                return Enabled && NodeUtil.IsBranchVisible(this);
            }
        }

        public bool TouchMove(Touch touch)
        {
            if (CanProcessTouch)
            {
                TouchMoveEvent.SendEvent();
                List<ITouchNode> list = clickNodes[touch];
                if (list.Count > 0)
                {
                    List<ITouchNode> nodes = GetNodes(touch);
                    List<ITouchNode> list2 = new();
                    foreach (ITouchNode touchNode in list)
                    {
                        if (nodes.IndexOf(touchNode) == -1)
                        {
                            touchNode.TouchOut(touch);
                            list2.Add(touchNode);
                        }
                    }
                    foreach (ITouchNode touchNode2 in list2)
                    {
                        list.Remove(touchNode2);
                    }
                }
                return OnTouchMove(touch);
            }
            return false;
        }

        public bool TouchBegin(Touch touch)
        {
            if (!CanProcessTouch)
            {
                return false;
            }
            TouchBeginEvent.SendEvent();
            List<ITouchNode> nodes = GetNodes(touch);
            foreach (ITouchNode touchNode in nodes)
            {
                touchNode.TouchBegan(touch);
            }
            clickNodes[touch] = nodes;
            return OnTouchBegin(touch);
        }

        public void TouchEnd(Touch touch)
        {
            if (CanProcessTouch)
            {
                TouchEndEvent.SendEvent();
                List<ITouchNode> nodes = GetNodes(touch);
                List<ITouchNode> list = clickNodes[touch];
                clickNodes.Remove(touch);
                foreach (ITouchNode touchNode in nodes)
                {
                    touchNode.TouchEnd(touch);
                    if (list.IndexOf(touchNode) != -1)
                    {
                        touchNode.Click(touch);
                        list.Remove(touchNode);
                    }
                    if (!CanProcessTouch)
                    {
                        break;
                    }
                }
                foreach (ITouchNode touchNode2 in list)
                {
                    touchNode2.TouchOut(touch);
                }
                OnTouchEnd(touch);
            }
        }

        protected override void OnAddedToStage()
        {
            Mokus2DGame.TouchController.AddListener(this, priority);
        }

        protected override void OnRemovedFromStage()
        {
            Mokus2DGame.TouchController.RemoveListener(this);
        }

        protected virtual bool OnTouchMove(Touch touch)
        {
            return true;
        }

        protected virtual bool OnTouchBegin(Touch touch)
        {
            return true;
        }

        protected virtual void OnTouchEnd(Touch touch)
        {
        }

        public List<ITouchNode> GetNodes(Touch touch)
        {
            List<ITouchNode> list = new();
            foreach (KeyValuePair<ITouchNode, Rectangle> keyValuePair in spriteRects)
            {
                Rectangle value = keyValuePair.Value;
                ITouchNode key = keyValuePair.Key;
                if (((Node)key).Visible)
                {
                    Vector2 vector = CocosUtil.TouchPointInNode(touch, (Node)key);
                    if (value.Contains((int)vector.X, (int)vector.Y))
                    {
                        list.Add(key);
                    }
                }
            }
            return list;
        }

        public override void RemoveChild(Node node)
        {
            if (node is ITouchNode && spriteRects.ContainsKey((ITouchNode)node))
            {
                spriteRects.Remove((ITouchNode)node);
            }
            base.RemoveChild(node);
        }

        public override void AddChild(Node node, int nodeLayer)
        {
            if (node is ITouchNode)
            {
                ITouchNode touchNode = (ITouchNode)node;
                Rectangle rectangle;
                if (node is IBoundsNode)
                {
                    rectangle = ((IBoundsNode)node).Bounds;
                }
                else
                {
                    rectangle = ClipFactory.GetBounds(touchNode);
                }
                spriteRects[touchNode] = rectangle;
            }
            base.AddChild(node, nodeLayer);
        }

        public readonly EventSender TouchBeginEvent = new();

        public readonly EventSender TouchEndEvent = new();

        public readonly EventSender TouchMoveEvent = new();
        protected Dictionary<Touch, List<ITouchNode>> clickNodes = new();

        protected Dictionary<ITouchNode, Rectangle> spriteRects = new();
    }
}

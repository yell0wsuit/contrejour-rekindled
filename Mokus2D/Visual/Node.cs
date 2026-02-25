using System;
using System.Collections.Generic;

using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual.Data;
using Mokus2D.Visual.GameDebug;

namespace Mokus2D.Visual
{
    public class Node : NodeBase, IUpdatable
    {
        public Node()
        {
            drawTracer = new GarbageTracer(GetType() + ".Draw", true);
        }

        public bool VisibleAndUpdating
        {
            set
            {
                UpdateEnabled = value;
                UpdateChildren = value;
                Visible = value;
            }
        }

        public NodeChildren Children => children;

        public int Layer => layer;

        public bool OnScreen => Root != null;

        public Node Parent
        {
            get => parent;
            protected set
            {
                parent = value;
                transformationDirty = true;
            }
        }

        public RootNode Root
        {
            get => root;
            internal set
            {
                if (root != value)
                {
                    bool flag = root == null && value != null;
                    bool flag2 = root != null && value == null;
                    root = value;
                    if (flag)
                    {
                        OnAddedToStage();
                        AddedToStage.Dispatch();
                    }
                    if (flag2)
                    {
                        OnRemovedFromStage();
                    }
                }
            }
        }

        public virtual bool IsRoot => Root == this;

        public event Action AddedToStage;

        public DelayedAction Schedule(Action action, float seconds)
        {
            DelayedAction delayedAction = new(action, seconds);
            Run(delayedAction);
            return delayedAction;
        }

        public override Vector2 Position
        {
            set
            {
                if (Position != value)
                {
                    base.Position = value;
                    matrixChanged = true;
                }
            }
        }

        public override float RotationRadians
        {
            set
            {
                if (RotationRadians != value)
                {
                    base.RotationRadians = value;
                    matrixChanged = true;
                }
            }
        }

        protected virtual void OnRemovedFromStage()
        {
        }

        protected virtual void OnAddedToStage()
        {
        }

        public virtual void Draw(VisualState state)
        {
        }

        public int GetChildIndex(Node child)
        {
            return children.IndexOf(child);
        }

        public void Run(NodeAction action)
        {
            action.Target = this;
            if (inActions)
            {
                actionsToAdd.Add(action);
                return;
            }
            actions.Add(action);
        }

        public void StopAction(NodeAction action)
        {
            if (inActions)
            {
                cachedActionsToRemove.Add(action);
                return;
            }
            _ = actions.Remove(action);
        }

        public void StopAllActions()
        {
            if (inActions)
            {
                hasToClearActions = true;
                return;
            }
            actions.Clear();
        }

        public virtual void ChangeChildLayer(Node node, int nodeLayer)
        {
            if (nodeLayer != node.layer)
            {
                node.layer = nodeLayer;
                _ = children.Remove(node);
                children.Add(node);
            }
        }

        public void AddChild(Node node)
        {
            AddChild(node, 0);
        }

        public virtual void AddChild(Node node, int nodeLayer)
        {
            if (node.Parent != null)
            {
                throw new InvalidOperationException("node already added to display list");
            }
            node.layer = nodeLayer;
            node.Parent = this;
            node.SetRoot(root);
            children.Add(node);
        }

        public Vector2 ZeroToNode(Node node, bool refreshTransformations = true)
        {
            return LocalToNode(Vector2.Zero, node, refreshTransformations);
        }

        public Vector2 LocalToNode(Vector2 source, Node node, bool refreshTransformations = true)
        {
            Vector2 vector = LocalToGlobal(source, refreshTransformations);
            return node.GlobalToLocal(vector, refreshTransformations);
        }

        public Vector2 GlobalToLocal(Vector2 source, bool refreshTransformations = true)
        {
            if (!OnScreen)
            {
                throw new InvalidOperationException("Node is not on display list");
            }
            if (refreshTransformations)
            {
                RefreshParentTransformations();
            }
            Matrix matrix = Matrix.Invert(compositeState.Matrix);
            return Vector2.Transform(source, matrix);
        }

        public Vector2 LocalToGlobal(Vector2 source, bool refreshTransformations = true)
        {
            if (refreshTransformations)
            {
                RefreshParentTransformations();
            }
            return Vector2.Transform(source, compositeState.Matrix);
        }

        private void RefreshParentTransformations()
        {
            cachedVisualStack.Clear();
            Node node = this;
            while (!node.IsRoot)
            {
                cachedVisualStack.Push(node);
                node = node.Parent;
            }
            node.RefreshVisualState(node.compositeState);
            VisualState visualState = node.compositeState;
            while (cachedVisualStack.Count != 0)
            {
                node = cachedVisualStack.Pop();
                node.RefreshVisualState(visualState);
                visualState = node.compositeState;
            }
        }

        internal void SetRoot(RootNode root)
        {
            Root = root;
            setRootCommand.Root = root;
            DoAllChildren(setRootCommand);
        }

        public void RemoveFromParent()
        {
            Parent.RemoveChild(this);
        }

        public virtual void RemoveChild(Node node)
        {
            node.Parent = null;
            node.SetRoot(null);
            if (!children.Remove(node))
            {
                throw new InvalidOperationException("There is no such children in collection");
            }
        }

        internal void TryUpdateNode(float time)
        {
            if (UpdateEnabled)
            {
                UpdateNode(time);
            }
        }

        public void UpdateNode(float time)
        {
            UpdateActions(time);
            using (new GarbageTracer(GetType(), true))
            {
                Update(time);
            }
            if (UpdateChildren)
            {
                tryUpdateCommand.Time = time;
                DoAllChildren(tryUpdateCommand);
            }
        }

        private void UpdateActions(float time)
        {
            cachedActionsToRemove.Clear();
            inActions = true;
            foreach (NodeAction nodeAction in actions)
            {
                nodeAction.Update(time);
                if (nodeAction.Finished)
                {
                    cachedActionsToRemove.Add(nodeAction);
                }
                if (hasToClearActions)
                {
                    break;
                }
            }
            inActions = false;
            if (hasToClearActions)
            {
                StopAllActions();
                hasToClearActions = false;
            }
            else
            {
                foreach (NodeAction nodeAction2 in cachedActionsToRemove)
                {
                    _ = actions.Remove(nodeAction2);
                }
            }
            actions.AddRange(actionsToAdd);
            actionsToAdd.Clear();
        }

        protected virtual void UpdateDrawProperties()
        {
            if (matrixChanged)
            {
                RefreshMatrix();
            }
        }

        private void RefreshMatrix()
        {
            nodeMatrix = Matrix.CreateScale(ScaleVec.X, ScaleVec.Y, 1f) * Matrix.CreateRotationZ(RotationRadians) * Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            matrixChanged = false;
            transformationDirty = true;
        }

        protected void DrawNode(VisualState state)
        {
            RefreshVisualState(state);
            int num = DrawChildrenPart(0, false);
            drawTracer.Start();
            Draw(compositeState);
            drawTracer.End();
            _ = DrawChildrenPart(num, true);
            compositeState.TransformationDirty = false;
        }

        private int DrawChildrenPart(int index, bool positiveLayers)
        {
            bool flag;
            do
            {
                flag = index < children.Count;
                if (flag)
                {
                    Node node = Children[index];
                    flag = flag && (positiveLayers || node.Layer < 0);
                    if (flag)
                    {
                        if (node.Visible)
                        {
                            node.DrawNode(compositeState);
                        }
                        index++;
                    }
                }
            }
            while (flag);
            return index;
        }

        private void RefreshVisualState(VisualState parentState)
        {
            UpdateDrawProperties();
            if (parentState.TransformationDirty || transformationDirty)
            {
                if (compositeState == null)
                {
                    compositeState = new VisualState(parentState);
                }
                compositeState.Refresh(parentState, nodeMatrix, OpacityFloat, Color, IgnoreParentOpacity, IgnoreParentColor);
                transformationDirty = false;
                return;
            }
            compositeState.RefreshValues(parentState, OpacityFloat, Color, IgnoreParentOpacity, IgnoreParentColor);
        }

        private void DoAllChildren(INodeCommand command)
        {
            DoChildren(command, allNodesReq);
        }

        private void DoVisibleChildren(INodeCommand command)
        {
            DoChildren(command, visibleNodesReq);
        }

        private void DoChildren(INodeCommand command, Func<Node, bool> req)
        {
            if (!req.Invoke(this))
            {
                return;
            }
            cachedChildrenCopy.Clear();
            cachedChildrenCopy.AddRange(Children);
            foreach (Node node in cachedChildrenCopy)
            {
                if (req.Invoke(node))
                {
                    command.Execute(node);
                }
            }
            cachedChildrenCopy.Clear();
        }

        public float X
        {
            get => Position.X; set => Position = new Vector2(value, Position.Y);
        }

        public float Y
        {
            get => Position.Y; set => Position = new Vector2(Position.X, value);
        }

        public override bool Visible
        {
            set
            {
                if (Visible != value)
                {
                    base.Visible = value;
                    transformationDirty = true;
                }
            }
        }

        public override Vector2 ScaleVec
        {
            get => base.ScaleVec;
            set
            {
                base.ScaleVec = value;
                matrixChanged = true;
            }
        }

        private const int DEFAULT_LIST_CAPACITY = 64;

        public bool UpdateChildren = true;

        private readonly List<NodeAction> actions = new();

        private readonly List<NodeAction> actionsToAdd = new();

        private readonly Func<Node, bool> allNodesReq = (Node node) => true;

        private readonly List<NodeAction> cachedActionsToRemove = new(64);

        private readonly List<Node> cachedChildrenCopy = new(64);

        private readonly Stack<Node> cachedVisualStack = new(64);

        private readonly NodeChildren children = new();

        private readonly SetRootCommand setRootCommand = new();

        private readonly TryUpdateCommand tryUpdateCommand = new();

        private readonly Func<Node, bool> visibleNodesReq = (Node node) => node.Visible;

        protected VisualState compositeState;

        private GarbageTracer drawTracer;

        private bool hasToClearActions;

        private bool inActions;

        private int layer;

        private bool matrixChanged;

        private Matrix nodeMatrix = Matrix.Identity;

        private Node parent;

        private RootNode root;

        private bool transformationDirty = true;
    }
}

using System.Collections.Generic;

namespace Mokus2D.Effects.Actions
{
    public abstract class MultipleNodeAction(params NodeAction[] actions) : NodeAction
    {
        public void Add(NodeAction action)
        {
            actions.Add(action);
        }

        internal override void Start(float time)
        {
            foreach (NodeAction action in actions)
            {
                action.Target = Target;
            }
        }

        protected readonly List<NodeAction> actions = new(actions);
    }
}

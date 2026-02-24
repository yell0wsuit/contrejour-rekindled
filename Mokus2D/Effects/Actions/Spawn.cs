namespace Mokus2D.Effects.Actions
{
    public class Spawn(params NodeAction[] actions) : MultipleNodeAction(actions)
    {
        internal override void Reset()
        {
            base.Reset();
            actions.ForEach(delegate (NodeAction action)
            {
                action.Reset();
            });
        }

        public override void Update(float time)
        {
            base.Update(time);
            bool flag = true;
            foreach (NodeAction nodeAction in actions)
            {
                if (!nodeAction.Finished)
                {
                    nodeAction.Update(time);
                    flag = false;
                }
            }
            finished = flag;
        }
    }
}

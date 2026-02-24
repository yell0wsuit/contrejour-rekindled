namespace Mokus2D.Effects.Actions
{
    public class Sequence(params NodeAction[] actions) : MultipleNodeAction(actions)
    {
        internal override void Reset()
        {
            base.Reset();
            actions.ForEach(delegate (NodeAction action)
            {
                action.Reset();
            });
            currentIndex = 0;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (currentAction == null)
            {
                currentAction = actions[currentIndex];
                currentIndex++;
            }
            currentAction.Update(time);
            if (currentAction.Finished)
            {
                currentAction = null;
                if (currentIndex == actions.Count)
                {
                    finished = true;
                }
            }
        }

        private NodeAction currentAction;

        private int currentIndex;
    }
}

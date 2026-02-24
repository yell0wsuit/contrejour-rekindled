namespace Mokus2D.Effects.Actions
{
    public class RepeatForever(NodeAction action) : NodeAction
    {
        internal override void Start(float time)
        {
            base.Start(time);
            action.Target = Target;
        }

        public override void Update(float time)
        {
            base.Update(time);
            action.Update(time);
            if (action.Finished)
            {
                action.Reset();
            }
        }
    }
}

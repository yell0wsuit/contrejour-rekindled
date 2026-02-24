namespace Mokus2D.Effects.Actions
{
    public abstract class EaseAction(IntervalActionBase action, float rate) : IntervalActionBase(action.Interval)
    {
        internal override void Start(float time)
        {
            base.Start(time);
            action.Target = Target;
            action.Start(time);
        }

        internal override void UpdateNode(float ratio)
        {
            action.UpdateNode(ratio);
        }

        protected float rate = rate;
    }
}

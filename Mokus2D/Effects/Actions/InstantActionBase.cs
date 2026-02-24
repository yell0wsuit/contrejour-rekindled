namespace Mokus2D.Effects.Actions
{
    public abstract class InstantActionBase : NodeAction
    {
        public override void Update(float time)
        {
            base.Update(time);
            Execute(time);
            finished = true;
        }

        protected abstract void Execute(float time);
    }
}

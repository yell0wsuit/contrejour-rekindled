namespace Mokus2D.Effects.Actions
{
    public class Remove : InstantActionBase
    {
        protected override void Execute(float time)
        {
            Target.RemoveFromParent();
        }
    }
}

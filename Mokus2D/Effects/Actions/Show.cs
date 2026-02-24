namespace Mokus2D.Effects.Actions
{
    public class Show : InstantActionBase
    {
        protected override void Execute(float time)
        {
            Target.Visible = true;
        }
    }
}

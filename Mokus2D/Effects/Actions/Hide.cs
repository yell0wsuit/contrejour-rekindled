namespace Mokus2D.Effects.Actions
{
    public class Hide : InstantActionBase
    {
        protected override void Execute(float time)
        {
            Target.Visible = false;
        }
    }
}

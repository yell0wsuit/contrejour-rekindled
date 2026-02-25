using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class CosRotationChanger(Node target, float maxValue, float step) : CosPropertyChanger(target, -maxValue, maxValue, step)
    {
        protected override void SetPropertyValue(float value)
        {
            ((Node)target).Rotation = value;
        }
    }
}

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public abstract class CosPropertyChanger : IUpdatable
    {
        public CosPropertyChanger(NodeBase target, float minValue, float maxValue, float step)
        {
            this.target = target;
            changer = new CosChanger(minValue, maxValue, step)
            {
                Progress = Maths.randRange(0f, 6.2831855f)
            };
        }

        public virtual float Value => changer.Value;

        public void Update(float time)
        {
            changer.Update(time);
            SetPropertyValue(Value);
        }

        protected abstract void SetPropertyValue(float value);

        protected CosChanger changer;

        protected NodeBase target;
    }
}

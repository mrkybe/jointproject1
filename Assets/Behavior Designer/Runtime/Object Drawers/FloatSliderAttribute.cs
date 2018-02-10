using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Object_Drawers
{
    public class FloatSliderAttribute : ObjectDrawerAttribute
    {
        public float min;
        public float max;

        public FloatSliderAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
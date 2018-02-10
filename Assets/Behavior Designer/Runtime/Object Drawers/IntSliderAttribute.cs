using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Object_Drawers
{
    public class IntSliderAttribute : ObjectDrawerAttribute
    {
        public int min;
        public int max;

        public IntSliderAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
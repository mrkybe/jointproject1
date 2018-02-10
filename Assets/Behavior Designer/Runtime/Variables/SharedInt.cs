using BehaviorDesigner.Runtime;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    [System.Serializable]
    public class SharedInt : SharedVariable<int>
    {
        public static implicit operator SharedInt(int value) { return new SharedInt { mValue = value }; }
    }
}
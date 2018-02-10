using Assets.Scripts.Classes.Mobile;
using BehaviorDesigner.Runtime;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    class SharedSpaceship : SharedVariable<Spaceship>
    {
        public static implicit operator SharedSpaceship(Spaceship value)
        {
            return new SharedSpaceship { Value = value };
        }
    }
}

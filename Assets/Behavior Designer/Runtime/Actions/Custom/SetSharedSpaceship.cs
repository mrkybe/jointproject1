using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    [TaskCategory("Basic/Spaceship")]
    [TaskDescription("Sets the variable Spaceship to the value Spaceship.")]
    class SetSharedSpaceship : Action
    {
        [Tooltip("The target Spaceship")]
        [RequiredField]
        public SharedSpaceship variable;
        [Tooltip("The value Spaceship")]
        public SharedSpaceship value;

        public override TaskStatus OnUpdate()
        {
            variable.Value = value.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            variable = null;
            value = null;
        }
    }
}

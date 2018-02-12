using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Mobile;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
    [TaskCategory("Basic/Math")]
    [TaskDescription("Performs a comparison between Spaceship and 'null'.")]
    public class IsSpaceshipNull : Conditional
    {
        [Tooltip("The spaceship.")]
        public SharedSpaceship spaceship;

        [Tooltip("Invert the result?")]
        public SharedBool invert;

        public override TaskStatus OnUpdate()
        {
            if(invert.Value == false)
                return spaceship.Value == null ? TaskStatus.Success : TaskStatus.Failure;
            else
                return spaceship.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            spaceship.Value = null;
            invert.Value = false;
        }
    }
}
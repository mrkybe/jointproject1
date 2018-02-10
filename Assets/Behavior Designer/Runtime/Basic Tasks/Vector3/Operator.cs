using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector3
{
    [TaskCategory("Basic/Vector3")]
    [TaskDescription("Performs a math operation on two Vector3s: Add, Subtract, Multiply, Divide, Min, or Max.")]
    public class Operator : Action
    {
        public enum Operation
        {
            Add,
            Subtract,
            Scale
        }

        [BehaviorDesigner.Runtime.Tasks.Tooltip("The operation to perform")]
        public Operation operation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first Vector3")]
        public SharedVector3 firstVector3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second Vector3")]
        public SharedVector3 secondVector3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            switch (operation) {
                case Operation.Add:
                    storeResult.Value = firstVector3.Value + secondVector3.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = firstVector3.Value - secondVector3.Value;
                    break;
                case Operation.Scale:
                    storeResult.Value = UnityEngine.Vector3.Scale(firstVector3.Value, secondVector3.Value);
                    break;
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            operation = Operation.Add;
            firstVector3 = secondVector3 = storeResult = UnityEngine.Vector3.zero;
        }
    }
}
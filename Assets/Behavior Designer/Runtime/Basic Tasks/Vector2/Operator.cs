using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Vector2
{
    [TaskCategory("Basic/Vector2")]
    [TaskDescription("Performs a math operation on two Vector2s: Add, Subtract, Multiply, Divide, Min, or Max.")]
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
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The first Vector2")]
        public SharedVector2 firstVector2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The second Vector2")]
        public SharedVector2 secondVector2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to store the result")]
        public SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            switch (operation) {
                case Operation.Add:
                    storeResult.Value = firstVector2.Value + secondVector2.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = firstVector2.Value - secondVector2.Value;
                    break;
                case Operation.Scale:
                    storeResult.Value = UnityEngine.Vector2.Scale(firstVector2.Value, secondVector2.Value);
                    break;
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            operation = Operation.Add;
            firstVector2 = secondVector2 = storeResult = UnityEngine.Vector2.zero;
        }
    }
}
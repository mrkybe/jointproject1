using System.Collections.Generic;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedTransformList values from the Transforms. Returns Success.")]
    public class SharedTransformsToTransformList : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The Transforms value")]
        public SharedTransform[] transforms;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedTransformList to set")]
        public SharedTransformList storedTransformList;

        public override void OnAwake()
        {
            storedTransformList.Value = new List<UnityEngine.Transform>();
        }

        public override TaskStatus OnUpdate()
        {
            if (transforms == null || transforms.Length == 0) {
                return TaskStatus.Failure;
            }

            storedTransformList.Value.Clear();
            for (int i = 0; i < transforms.Length; ++i) {
                storedTransformList.Value.Add(transforms[i].Value);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            transforms = null;
            storedTransformList = null;
        }
    }
}
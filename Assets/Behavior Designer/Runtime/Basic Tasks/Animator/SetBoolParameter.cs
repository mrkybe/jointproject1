using System.Collections;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Sets the bool parameter on an animator. Returns Success.")]
    public class SetBoolParameter : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the parameter")]
        public SharedString paramaterName;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The value of the bool parameter")]
        public SharedBool boolValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Should the value be reverted back to its original value after it has been set?")]
        public bool setOnce;

        private int hashID;
        private UnityEngine.Animator animator;
        private UnityEngine.GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                animator = currentGameObject.GetComponent<UnityEngine.Animator>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (animator == null) {
                UnityEngine.Debug.LogWarning("Animator is null");
                return TaskStatus.Failure;
            }

            hashID = UnityEngine.Animator.StringToHash(paramaterName.Value);

            bool prevValue = animator.GetBool(hashID);
            animator.SetBool(hashID, boolValue.Value);
            if (setOnce) {
                StartCoroutine(ResetValue(prevValue));
            }
            return TaskStatus.Success;
        }

        public IEnumerator ResetValue(bool origVale)
        {
            yield return null;
            animator.SetBool(hashID, origVale);
        }

        public override void OnReset()
        {
            targetGameObject = null;
            paramaterName = "";
            boolValue = false;
        }
    }
}
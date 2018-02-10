using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Animator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress. Returns Success.")]
    public class MatchTarget : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The position we want the body part to reach")]
        public SharedVector3 matchPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The rotation in which we want the body part to be")]
        public SharedQuaternion matchRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The body part that is involved in the match")]
        public AvatarTarget targetBodyPart;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Weights for matching position")]
        public UnityEngine.Vector3 weightMaskPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Weights for matching rotation")]
        public float weightMaskRotation;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Start time within the animation clip")]
        public float startNormalizedTime;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("End time within the animation clip")]
        public float targetNormalizedTime = 1;

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

            animator.MatchTarget(matchPosition.Value, matchRotation.Value, targetBodyPart, new MatchTargetWeightMask(weightMaskPosition, weightMaskRotation), startNormalizedTime, targetNormalizedTime);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            matchPosition = UnityEngine.Vector3.zero;
            matchRotation = UnityEngine.Quaternion.identity;
            targetBodyPart = AvatarTarget.Root;
            weightMaskPosition = UnityEngine.Vector3.zero;
            weightMaskRotation = 0;
            startNormalizedTime = 0;
            targetNormalizedTime = 1;
        }
    }
}
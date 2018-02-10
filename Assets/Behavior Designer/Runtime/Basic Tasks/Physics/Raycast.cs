using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Physics
{
    [TaskCategory("Basic/Physics")]
    [TaskDescription("Casts a ray against all colliders in the scene. Returns success if a collider was hit.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=117")]
    public class Raycast : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Starts the ray at the GameObject's position. If null the originPosition will be used")]
        public SharedGameObject originGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Starts the ray at the position. Only used if originGameObject is null")]
        public SharedVector3 originPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The direction of the ray")]
        public SharedVector3 direction;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The length of the ray. Set to -1 for infinity")]
        public SharedFloat distance = -1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Selectively ignore colliders")]
        public UnityEngine.LayerMask layerMask = -1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Cast the ray in world or local space. The direction is in world space if no GameObject is specified")]
        public Space space = Space.Self;

        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit object of the raycast")]
        public SharedGameObject storeHitObject;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit point of the raycast")]
        public SharedVector3 storeHitPoint;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit normal of the raycast")]
        public SharedVector3 storeHitNormal;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit distance of the raycast")]
        public SharedFloat storeHitDistance;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Vector3 position;
            UnityEngine.Vector3 dir = direction.Value;
            if (originGameObject.Value != null) {
                position = originGameObject.Value.transform.position;
                if (space == Space.Self) {
                    dir = originGameObject.Value.transform.TransformDirection(direction.Value);
                }
            } else {
                position = originPosition.Value;
            }

            RaycastHit hit;
            if (UnityEngine.Physics.Raycast(position, dir, out hit, distance.Value == -1 ? Mathf.Infinity : distance.Value, layerMask)) {
                storeHitObject.Value = hit.collider.gameObject;
                storeHitPoint.Value = hit.point;
                storeHitNormal.Value = hit.normal;
                storeHitDistance.Value = hit.distance;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            originGameObject = null;
            originPosition = UnityEngine.Vector3.zero;
            direction = UnityEngine.Vector3.zero;
            distance = -1;
            layerMask = -1;
            space = Space.Self;
        }
    }
}
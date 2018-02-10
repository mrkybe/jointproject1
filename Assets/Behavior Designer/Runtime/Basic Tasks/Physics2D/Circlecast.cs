using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.Physics2D
{
    [TaskCategory("Basic/Physics2D")]
    [TaskDescription("Casts a circle against all colliders in the scene. Returns success if a collider was hit.")]
    [BehaviorDesigner.Runtime.Tasks.HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=118")]
    public class Circlecast : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Starts the circlecast at the GameObject's position. If null the originPosition will be used.")]
        public SharedGameObject originGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Starts the circlecast at the position. Only used if originGameObject is null.")]
        public SharedVector2 originPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The radius of the circlecast")]
        public SharedFloat radius;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The direction of the circlecast")]
        public SharedVector2 direction;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The length of the ray. Set to -1 for infinity.")]
        public SharedFloat distance = -1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Selectively ignore colliders.")]
        public UnityEngine.LayerMask layerMask = -1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Use world or local space. The direction is in world space if no GameObject is specified.")]
        public Space space = Space.Self;

        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit object of the circlecast.")]
        public SharedGameObject storeHitObject;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit point of the circlecast.")]
        public SharedVector2 storeHitPoint;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit normal of the circlecast.")]
        public SharedVector2 storeHitNormal;
        [SharedRequired]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit distance of the circlecast.")]
        public SharedFloat storeHitDistance;

        public override TaskStatus OnUpdate()
        {
            UnityEngine.Vector2 position;
            UnityEngine.Vector2 dir = direction.Value;
            if (originGameObject.Value != null) {
                position = originGameObject.Value.transform.position;
                if (space == Space.Self) {
                    dir = originGameObject.Value.transform.TransformDirection(direction.Value);
                }
            } else {
                position = originPosition.Value;
            }

            var hit = UnityEngine.Physics2D.CircleCast(position, radius.Value, dir, distance.Value == -1 ? Mathf.Infinity : distance.Value, layerMask);
            if (hit.collider != null) {
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
            originPosition = UnityEngine.Vector2.zero;
            direction = UnityEngine.Vector2.zero;
            radius = 0;
            distance = -1;
            layerMask = -1;
            space = Space.Self;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Behavior_Designer.Runtime.Variables;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Assets.Behavior_Designer.Runtime.Actions.Custom
{
    /// <summary>
    /// Action to calculate obstacles and find paths.
    /// </summary>
    class CalculateSteering : Action
    {
        public SharedInt RaysToCast = 1;
        public SharedFloat CastDistance = 100;
        public SharedFloat SideImportance = 1.0f;
        public SharedFloat ConeSize = 50;
        public SharedBool ShowDebugLines = true;
        
        public RaycastHit[] Hits;

        private LayerMask layerMask = new LayerMask();

        public override void OnAwake()
        {
            base.OnAwake();
            Hits = new RaycastHit[RaysToCast.Value];
        }

        public override TaskStatus OnUpdate()
        {
            for(int i = 0; i < RaysToCast.Value; i++)
            {
                RaycastHit h = Hits[i];

                float angle = i - ((RaysToCast.Value - 1) / 2.0f);
                Vector3 direction = transform.forward;
                direction = Quaternion.Euler(0, angle * ConeSize.Value, 0) * direction;
                
                Physics.Raycast(transform.position, direction, out h, CastDistance.Value, layerMask);
                if (ShowDebugLines.Value)
                {
                    if (h.transform == null)
                    {
                        Debug.DrawLine(transform.position + direction * CastDistance.Value, transform.position, Color.green, 0.2f);
                    }
                    else
                    {
                        Debug.DrawLine(h.point, transform.position, Color.green, 0.2f);
                    }
                }
            }
            return TaskStatus.Success;
        }
    }
}

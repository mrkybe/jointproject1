using System.Collections.Generic;
using Assets.Behavior_Designer.Runtime.Variables;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Behavior_Designer.Runtime.Basic_Tasks.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedGameObjectList values from the GameObjects. Returns Success.")]
    public class SharedGameObjectsToGameObjectList : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObjects value")]
        public SharedGameObject[] gameObjects;
        [RequiredField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The SharedTransformList to set")]
        public SharedGameObjectList storedGameObjectList;

        public override void OnAwake()
        {
            storedGameObjectList.Value = new List<UnityEngine.GameObject>();
        }

        public override TaskStatus OnUpdate()
        {
            if (gameObjects == null || gameObjects.Length == 0) {
                return TaskStatus.Failure;
            }

            storedGameObjectList.Value.Clear();
            for (int i = 0; i < gameObjects.Length; ++i) {
                storedGameObjectList.Value.Add(gameObjects[i].Value);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            gameObjects = null;
            storedGameObjectList = null;
        }
    }
}
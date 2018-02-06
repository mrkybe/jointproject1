using BehaviorDesigner.Runtime;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    class SharedAsteroid : SharedVariable<AsteroidField>
    {
        public static implicit operator SharedAsteroid(AsteroidField value)
        {
            return new SharedAsteroid { Value = value };
        }
    }
}

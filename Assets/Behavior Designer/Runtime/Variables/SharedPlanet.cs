using BehaviorDesigner.Runtime;

namespace Assets.Behavior_Designer.Runtime.Variables
{
    class SharedPlanet : SharedVariable<Planet>
    {
        public static implicit operator SharedPlanet(Planet value)
        {
            return new SharedPlanet { Value = value };
        }
    }
}

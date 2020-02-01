using Leopotam.Ecs;
using Modules.Positioning.Components;

namespace Modules.Positioning.Systems
{
    public sealed class URotationsSyncSystem : IEcsRunSystem
    {
        private EcsFilter<URotationRoot, Position> _filter;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                _filter.Get1[i].Transform.eulerAngles = _filter.Get2[i].EulerRotation;
            }
        }
    }
}